using System;
using Modules.EntityModule.Scripts.Movement.TargetPoint;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Movement.Path
{
    public class PathMovementModel
    {
        public TargetPointMovementModel TargetPointMovementModel { get; }
        public event Action EndedAllPath;

        private int _targetPointIndex;

        private readonly PathComponent _pathComponent;

        public PathMovementModel(PathComponent pathComponent)
        {
            _pathComponent = pathComponent;
            TargetPointMovementModel = new TargetPointMovementModel();
        }

        public void TryIncreaseTargetPointIndex()
        {
            if (_targetPointIndex + 1 > _pathComponent.MovementPositionsTransforms.Length)
            {
                return;
            }
            
            _targetPointIndex++;

            if (_targetPointIndex == _pathComponent.MovementPositionsTransforms.Length)
            {
                EndedAllPath?.Invoke();
            }
        }

        public Vector3 GetTargetPointPosition()
        {
            if (_pathComponent.MovementPositionsTransforms.Length == 0)
                return Vector3.zero;

            return DoesEndPath()
                ? _pathComponent.MovementPositionsTransforms[^1].position
                : _pathComponent.MovementPositionsTransforms[_targetPointIndex].position;
        }

        public bool DoesEndPath()
        {
            return _targetPointIndex >= _pathComponent.MovementPositionsTransforms.Length;
        }
    }
}