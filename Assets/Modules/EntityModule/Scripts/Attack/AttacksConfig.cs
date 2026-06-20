using System;
using System.Collections.Generic;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Attack
{
    [Serializable]
    public class AttacksConfig<TAttackType, TCustomAttackConfig> 
        where TAttackType : Enum
        where TCustomAttackConfig : ICustomAttackConfig
    {
        [field: SerializeField] public float AttackDistance { get; private set; }
        [field: SerializeField] public float CooldownBetweenAttacks { get; private set; }
        [field: SerializeField] public List<AttackConfig<TAttackType, TCustomAttackConfig>> AttacksConfigs { get; private set; }
    }
}