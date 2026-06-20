using System;

namespace Modules.EntityModule.Scripts.Attack
{
    public interface IAttackModel
    {
        event Action StartedAttackWithoutArgs, EndedAttackWithoutArgs;
    }
}