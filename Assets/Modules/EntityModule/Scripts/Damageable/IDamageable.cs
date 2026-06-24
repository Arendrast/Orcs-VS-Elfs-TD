using System;

namespace Modules.EntityModule.Scripts.Damageable
{
    public interface IDamageable
    {
        event Action Died;
        bool IsDied { get; }
        bool TryTakeDamage(int damage);
        void SetIsDamageable(bool value);
    }
}