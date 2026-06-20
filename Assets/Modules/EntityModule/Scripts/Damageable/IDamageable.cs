namespace Modules.EntityModule.Scripts.Damageable
{
    public interface IDamageable
    {
        bool IsDied { get; }
        bool TryTakeDamage(int damage);
    }
}