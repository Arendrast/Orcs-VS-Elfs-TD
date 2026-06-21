using System;
using System.Collections.Generic;
using System.Linq;
using Modules.EntityModule.Scripts.Damageable;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Attack.FollowNearestDamageable
{
    public class SelectDamageableModel
    {
        public enum SelectTargetType
        {
            NearestAlive,
            FirstAlive
        }

        public IReadOnlyDictionary<Transform, TargetData> PotentialTargetsDataInAttackZone =>
            _potentialTargetsDataInAttackZone;

        public TargetData? TargetData { get; private set; }

        public SelectTargetType SelectType { get; }
        
        public event Action<TargetData?> UpdatedTargetData;

        private readonly Dictionary<Transform, TargetData> _potentialTargetsDataInAttackZone =
            new Dictionary<Transform, TargetData>();
       
        private readonly Func<Vector3> _positionFunc;

        public SelectDamageableModel(Func<Vector3> positionFunc, SelectTargetType selectTargetType)
        {
            _positionFunc = positionFunc;
            SelectType = selectTargetType;
        }

        public void TryAddPotentialTargetInAttackZone(Transform potentialTarget,
            IDamageable damageable)
        {
            if (_potentialTargetsDataInAttackZone.ContainsKey(potentialTarget))
            {
                return;
            }

            var data = new TargetData(potentialTarget, damageable);

            _potentialTargetsDataInAttackZone.Add(potentialTarget, data);
            
            if (TargetData == null)
            {
                TargetData = data;
                UpdatedTargetData?.Invoke(TargetData);
            }
        }

        public void TryRemovePotentialTargetInAttackZone(Transform potentialTarget)
        {
            _potentialTargetsDataInAttackZone.Remove(potentialTarget);

            if (_potentialTargetsDataInAttackZone.Count == 0 && TargetData != null)
            {
                TargetData = null;
                UpdatedTargetData?.Invoke(TargetData);
            }
        }

        public Vector3 GetDirectionToTarget()
        {
            return (TargetData.Value.Transform.position - _positionFunc.Invoke()).normalized;
        }

        public void AppointTarget()
        {
            switch (SelectType)
            {
                case SelectTargetType.FirstAlive:
                    AppointTargetAsFirstAlive();
                    break;
                case SelectTargetType.NearestAlive:
                    AppointTargetAsNearestAlive();
                    break;
            }
        }

        private void AppointTargetAsFirstAlive()
        {
            TargetData = null;
            
            foreach (var target in _potentialTargetsDataInAttackZone.Values)
            {
                if (target.Damageable.IsDied)
                {
                    continue;
                }
                
                TargetData = target;
                return;
            }
        }

        private void AppointTargetAsNearestAlive()
        {
            var minimalDistance = float.MaxValue;

            var currentTargetData = TargetData;
            TargetData = null;

            foreach (var target in _potentialTargetsDataInAttackZone.Values)
            {
                if (target.Damageable.IsDied)
                {
                    continue;
                }
                
                var distance = (_positionFunc.Invoke() - target.Transform.position).sqrMagnitude;

                if (distance < minimalDistance)
                {
                    minimalDistance = distance;
                    TargetData = target;
                }
            }

            if (!currentTargetData.Equals(TargetData))
            {
                UpdatedTargetData?.Invoke(TargetData);
            }
        }
    }
}