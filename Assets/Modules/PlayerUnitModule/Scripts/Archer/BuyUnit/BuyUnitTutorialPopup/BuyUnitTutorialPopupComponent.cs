using Modules.PlayerUnitModule.Scripts.Archer.BuyUnit.BuyUnitPopup;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Archer.BuyUnit.BuyUnitTutorialPopup
{
    public class BuyUnitTutorialPopupComponent : MonoBehaviour
    {
        public BuyUnitTutorialPopupController BuyUnitTutorialPopupController { get; private set; }

        [SerializeField] private BuyUnitTutorialPopupConfig _config;

        private void Awake()
        {
            BuyUnitTutorialPopupController = new BuyUnitTutorialPopupController(_config, gameObject);
        }

        private void OnDisable()
        {
            BuyUnitTutorialPopupController?.Dispose();
        }
    }
}