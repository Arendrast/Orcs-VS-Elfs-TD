using UnityEngine;

namespace Modules.SharedModule.Scripts.Currencies.CurrenciesPopup
{
    public class CurrenciesPopupComponent : MonoBehaviour
    {
        public CurrenciesPopupController Controller { get; private set; }
        [SerializeField] private CurrenciesPopupConfig _config;

        public void Construct(CurrencyRepositoryService currencyRepositoryService)
        {
            Controller = new CurrenciesPopupController(currencyRepositoryService, gameObject, _config);
        }

        private void OnEnable()
        {
            Controller?.Open();
        }

        private void OnDisable()
        {
            Controller?.Close();
        }
    }
}