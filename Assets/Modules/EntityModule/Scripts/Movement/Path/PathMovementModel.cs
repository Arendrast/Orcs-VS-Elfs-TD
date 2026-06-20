using System;
using Modules.EntityModule.Scripts.Movement.TargetPoint;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Movement.Path
{
    public class PathMovementModel
    {
        public event Action EndedAllPath;
        public TargetPointMovementModel TargetPointMovementModel { get; }

        private int _targetPointIndex;

        private readonly PathConfig _pathConfig;

        public PathMovementModel(PathConfig pathConfig)
        {
            _pathConfig = pathConfig;
            TargetPointMovementModel = new TargetPointMovementModel();
        }

        public void TryIncreaseTargetPointIndex()
        {
            if (_targetPointIndex + 1 > _pathConfig.MovementPositionsTransforms.Length)
            {
                return;
            }
            
            _targetPointIndex++;

            if (_targetPointIndex == _pathConfig.MovementPositionsTransforms.Length)
            {
                EndedAllPath?.Invoke();
            }
        }

        public Vector3 GetTargetPointPosition()
        {
            if (_pathConfig.MovementPositionsTransforms.Length == 0)
                return Vector3.zero;

            return DoesEndPath()
                ? _pathConfig.MovementPositionsTransforms[^1].position
                : _pathConfig.MovementPositionsTransforms[_targetPointIndex].position;
        }

        public bool DoesEndPath()
        {
            return _targetPointIndex >= _pathConfig.MovementPositionsTransforms.Length;
        }
    }
}