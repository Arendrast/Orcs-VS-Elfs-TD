using Modules.EnemyModule.Scripts;
using Modules.EntityModule.Scripts.Attack;
using Modules.EntityModule.Scripts.Damageable;
using Modules.SharedModule;
using UnityEngine;

namespace Modules.CoreModule.Scripts
{
    public class GameplayComponents : MonoBehaviour 
    {
        [field: SerializeField] public DamageableComponent[] EnemyDamageableComponents { get; private set; }
        [field: SerializeReference] public AttackComponent[] EnemyAttackComponents { get; private set; }
        [field: SerializeReference] public ActivatorAfterDelayComponent[] EnemyActivatorAfterDelayComponents { get; private set; }

        [field: SerializeReference] public DamageableComponent[] PlayerDamageableComponents { get; private set; }
        [field: SerializeReference] public AttackComponent[] PlayerAttackComponents { get; private set; }
        // Чтобы не изменять код и добавлять новых мобов/юнитов - указываю компоненты, а не ссылки на конкретных мобов
    }
}