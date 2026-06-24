using DG.Tweening;
using Modules.EntityModule.Scripts.Movement.TargetPoint;
using Modules.SharedModule;
using Modules.SharedModule.Scripts;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Movement.Path
{
    public class PathMovementController
    {
        public TargetPointMovementController PointMovementController { get; }
        public PathMovementModel Model { get; }
        private readonly Transform _transform;
        private readonly PathMovementConfig _pathConfig;

        public PathMovementController(PathMovementModel model, Transform transform, PathMovementConfig pathConfig)
        {
            Model = model;
            _transform = transform;
            _pathConfig = pathConfig;
            PointMovementController = new TargetPointMovementController(transform, model.TargetPointMovementModel, OnEndMove);
        }

        public bool TryStartMoveToPoints() 
            // Я думал над тем, чтобы реализовать движение через character controller или transforms.Translate,
            // но поскольку физики нет, посчитал, что использовать здесь дутвин очень подходит - оптимизированнее и местами даже читабельнее
        {
            if (Model.TargetPointMovementModel.DoesMove || Model.DoesEndPath())
            {
                return false;
            }

            var targetPosition = Model.GetTargetPointPosition();

            var sqrDistance =
                (targetPosition - _transform.position)
                .sqrMagnitude; // Использую квадрат дистанции, чтобы не считать корень

            if (sqrDistance < ConstantsHolder.SqrEpsilon)
            {
                Model.TryIncreaseTargetPointIndex();

                targetPosition = Model.GetTargetPointPosition();
            }
            
            PointMovementController.MoveToPoint(targetPosition, _pathConfig.Speed);

            return true;
        }

        public void TryStopMove()
        {
            PointMovementController.TryStopMove();
        }

        private void OnEndMove()
        {
            Model.TryIncreaseTargetPointIndex();

            if (!TryStartMoveToPoints())
            {
                Model.TargetPointMovementModel.SetDoesMove(false);
            }
        }
    }
}