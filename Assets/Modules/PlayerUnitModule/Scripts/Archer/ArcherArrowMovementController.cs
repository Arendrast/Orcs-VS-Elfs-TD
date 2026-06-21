using System;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Archer
{
    public class ArcherArrowMovementController
    {
        public Transform Transform { get; }
        
        private float _elapsedTime = 0f;
        private Vector3 _lastTargetPosition;

        private readonly Transform _target;
        private readonly Vector3 _startPosition;
        private readonly float _duration;
        private readonly Action _endedMovement;

        public ArcherArrowMovementController(Transform target, float flyTime, Vector3 startPosition,
            Transform transform, Action endedMovement)
        {
            Transform = transform;
            _endedMovement = endedMovement;
            _target = target;
            _duration = flyTime;
            _startPosition = startPosition;

            if (target != null)
            {
                _lastTargetPosition = target.position;
            }

            TryMove(0); // Чтобы сразу был виден поворот/позиция
        }

        public bool TryMove(float deltaTime)
        {
            var progress = GetProgress();

            if (progress >= 1f)
            {
                return false;
            }

            if (_target != null)
            {
                _lastTargetPosition = new Vector3(_target.position.x, _startPosition.y, _target.position.z);
            }

            _elapsedTime += deltaTime;

            progress = GetProgress();

            progress = Mathf.Clamp01(progress);

            var currentTargetPosition = _lastTargetPosition;
            
            Transform.position = progress == 0
                ? _startPosition
                : Vector3.Lerp(_startPosition, currentTargetPosition, progress);

            var direction = currentTargetPosition - Transform.position;

            if (direction != Vector3.zero)
            {
                Transform.rotation = Quaternion.LookRotation(direction);
            }

            if (progress >= 1f)
            {
                OnEndMove();
            }

            return true;
        }

        private float GetProgress()
        {
            return _elapsedTime == 0 ? 0 : _elapsedTime / _duration;
        }

        private void OnEndMove()
        {
            _endedMovement?.Invoke();
        }
    }
}