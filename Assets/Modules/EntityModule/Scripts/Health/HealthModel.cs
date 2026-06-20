using System;
using UnityEngine;

namespace Modules.EntityModule.Scripts.Health
{
    public class HealthModel
    {
        public int MaxHealthPoints { get; private set; }
        public int HealthPoints { get; private set; }
        
        public bool IsDied { get; private set; }
        
        public event Action Died;
        public event Action<int> ChangedHealthPoints;
        
        public HealthModel(int maxHealth)
        {
            MaxHealthPoints = maxHealth;
            HealthPoints = maxHealth;
        }

        public bool TrySetHealthPoints(int newHealthPoints)
        {
            if (IsDied || newHealthPoints < 0)
            {
                return false;
            }

            newHealthPoints = Mathf.Min(MaxHealthPoints, newHealthPoints);
            
            if (newHealthPoints == HealthPoints)
            {
                return false;
            }
            
            HealthPoints = newHealthPoints;
            
            if (newHealthPoints == 0)
            {
                IsDied = true;
                Died?.Invoke();
            }
            
            ChangedHealthPoints?.Invoke(newHealthPoints);

            return true;
        }
    }
}