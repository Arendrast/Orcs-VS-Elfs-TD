using Modules.EntityModule.Scripts.Damageable;
using Modules.SharedModule.Scripts.Currencies;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Archer.BuyUnit.ShowMoneyPopup
{
    public class ShowMoneyPopupComponent : MonoBehaviour
    {
        public ShowMoneyPopupController Controller { get; private set; }
        [SerializeField] private ShowMoneyPopupConfig _config;

        public void Construct(MoneyImageFactory factory, DamageablesRepository repository,
            Camera mainCamera, BuyMergeUnitConfig buyMergeUnitConfig,
            CurrencyRepositoryService currencyRepositoryService)
        {
            Controller = new ShowMoneyPopupController(_config, factory, repository, mainCamera, buyMergeUnitConfig,
                currencyRepositoryService, this);
        }

        private void OnDisable()
        {
            Controller?.Dispose();
        }
    }
}