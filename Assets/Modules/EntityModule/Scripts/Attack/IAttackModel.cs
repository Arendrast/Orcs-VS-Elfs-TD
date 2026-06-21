using System;

namespace Modules.EntityModule.Scripts.Attack
{
    public interface IAttackModel
    {
        TargetData? TargetData { get; }
        event Action<IAttackConfig> StartedAttackByConfig, EndedAttackByConfig;
    }
}