using Modules.EntityModule.Scripts.Attack;

namespace Modules.PlayerUnitModule.Scripts.Archer
{
    public class PlayerArcherAttackComponent : AttackComponent<PlayerArcherAttackType, EmptyCustomAttackConfig>
    {
        public bool CanSkipAttack()
        {
            return !AttackModel.TargetData.HasValue || !AttackController.DealtDamageInCurrentAttack &&
                AttackModel.TargetData.Value.Damageable.IsDied;
        }
    }
}