using System;
using Modules.SharedModule.Scripts.Audio;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Archer.BuyUnit
{
    public class BuyUnitSoundsComponent : MonoBehaviour
    {
        public BuyUnitSoundsController BuyUnitSoundsController { get; private set; }

        public void Construct(AudioService audioService)
        {
            
        }
    }
}