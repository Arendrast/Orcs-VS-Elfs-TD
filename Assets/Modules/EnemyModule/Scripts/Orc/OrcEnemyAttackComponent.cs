using Modules.EntityModule.Scripts.Attack;

namespace Modules.EnemyModule.Scripts.Orc
{
    public class OrcEnemyAttackComponent : AttackComponent<OrcEnemyAttackType, EmptyCustomAttackConfig>
    {
        public bool CanSkipAttack()
        {
            return !AttackModel.TargetData.HasValue || !AttackController.DealtDamageInCurrentAttack &&
                AttackModel.TargetData.Value.Damageable.IsDied;
        }
    }
}