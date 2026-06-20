using System;
using Modules.EntityModule.Scripts.Damageable;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Attack
{
    public readonly struct TargetData : IEquatable<TargetData>
    {
        public readonly Transform Transform;
        public readonly IDamageable Damageable;

        public TargetData(Transform transform, IDamageable damageable)
        {
            Transform = transform;
            Damageable = damageable;
        }

        public bool Equals(TargetData other)
        {
            return Equals(Transform, other.Transform) && Equals(Damageable, other.Damageable);
        }

        public override bool Equals(object obj)
        {
            return obj is TargetData other && Equals(other);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Transform, Damageable);
        }
    }
}