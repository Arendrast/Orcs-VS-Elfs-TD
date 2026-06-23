using UnityEngine;

namespace Modules.SharedModule.Scripts
{
    public class TimeScaleRepositoryService
    {
        public float TimeScale => Time.timeScale;
        
        public void SetTimeScale(float timeScale)
        {
            Time.timeScale = timeScale;
        }
    }
}