using System;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Attack
{
    public interface IAttackConfig
    {
        float FullAttackTime { get; }
        float DoDamageTime { get; }
        int Damage { get; }
    }
    
    [Serializable]
    public class AttackConfig<TAttackType, TCustomData>  : IAttackConfig
        where TAttackType : Enum
        where TCustomData : ICustomAttackConfig
    {
        [field: SerializeField] public float FullAttackTime { get; private set; }
        [field: SerializeField] public float DoDamageTime { get; private set; }
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public TAttackType AttackType { get; private set; }
        [field: SerializeField] public TCustomData AdditionalConfig { get; private set; }
    }
}