using DG.Tweening;
using Modules.EntityModule.Scripts.Movement.TargetPoint;
using Modules.SharedModule;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Movement.Path
{
    public class PathMovementController
    {
        public TargetPointMovementController PointMovementController { get; }
        private readonly PathMovementModel _model;
        private readonly Transform _transform;
        private readonly PathMovementConfig _pathConfig;

        public PathMovementController(PathMovementModel model, Transform transform, PathMovementConfig pathConfig)
        {
            _model = model;
            _transform = transform;
            _pathConfig = pathConfig;
            PointMovementController = new TargetPointMovementController(transform, model.TargetPointMovementModel, OnEndMove);

            if (pathConfig.StartMovementOnAwake)
            {
                TryStartMoveToPoints();
            }
        }

        public bool TryStartMoveToPoints() 
            // Я думал над тем, чтобы реализовать движение через character controller или transforms.Translate,
            // но поскольку физики нет, посчитал, что использовать здесь дутвин очень подходит - оптимизированнее и местами даже читабельнее
        {
            if (_model.TargetPointMovementModel.DoesMove || _model.DoesEndPath())
            {
                return false;
            }

            var targetPosition = _model.GetTargetPointPosition();

            var sqrDistance =
                (targetPosition - _transform.position)
                .sqrMagnitude; // Использую квадрат дистанции, чтобы не считать корень

            if (sqrDistance < ConstantsHolder.SqrEpsilon)
            {
                _model.TryIncreaseTargetPointIndex();

                targetPosition = _model.GetTargetPointPosition();
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
            _model.TryIncreaseTargetPointIndex();

            if (!TryStartMoveToPoints())
            {
                _model.TargetPointMovementModel.SetDoesMove(false);
            }
        }
    }
}