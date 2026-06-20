using System;
using DG.Tweening;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Movement.TargetPoint
{
    public class TargetPointMovementController
    {
        public TargetPointMovementModel Model { get; }
        private readonly Transform _transform;
        private readonly Action _completedMove;

        public TargetPointMovementController(Transform transform, TargetPointMovementModel model, Action completedMove)
        {
            _transform = transform;
            Model = model;
            _completedMove = completedMove;
        }

        public void TryStopMove()
        {
            if (Model.DoesMove)
            {
                _transform.DOKill();
            }
        }

        public void MoveToPoint(Vector3 targetPosition, float speed, bool rotate = true)
        {
            _transform.DOKill();
            
            Model.TargetPoint = targetPosition;
            Model.TargetSpeed = speed;
            
            Model.SetDoesMove(true);
            
            var direction = targetPosition - _transform.position;

            if (rotate)
            {
                _transform.forward = new Vector3(direction.x, 0, direction.z);
            }
            
            _transform.DOMove(targetPosition, speed).SetSpeedBased().OnComplete(OnCompleteMove).OnKill(OnStopMove)
                .SetEase(Ease.Linear);
        }

        private void OnStopMove()
        {
            Model.SetDoesMove(false);
        }

        private void OnCompleteMove()
        {
            Model.SetDoesMove(false);
            _completedMove?.Invoke();
        }
    }
}