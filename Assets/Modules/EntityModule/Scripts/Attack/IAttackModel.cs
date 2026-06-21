using System;

namespace Modules.EntityModule.Scripts.Attack
{
    public interface IAttackModel
    {
        bool IsAttacking { get; }
        IAttackConfig TargetAttackConfig { get; }
        TargetData? TargetData { get; }
        event Action<IAttackConfig> StartedAttackByConfig, EndedAttackByConfig;
    }
}