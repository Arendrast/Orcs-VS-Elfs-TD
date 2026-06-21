using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Archer
{
    public class ArcherArrowMovementComponent : MonoBehaviour
    {
        private ArcherArrowMovementController _archerArrowMovementController;
        
        public void Construct(ArcherArrowMovementController archerArrowMovementController)
        {
            _archerArrowMovementController = archerArrowMovementController;
        }
        
        private void Update()
        {
            _archerArrowMovementController.TryMove(Time.deltaTime);
        }
    }
}