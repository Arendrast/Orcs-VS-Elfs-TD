using System;
using UnityEngine;

namespace Modules.SharedModule.Scripts.Currencies.CurrenciesPopup
{
    public class CurrenciesPopupController : IDisposable
    {
        private readonly CurrencyRepositoryService _repositoryService;
        private readonly GameObject _gameObject;
        private readonly CurrenciesPopupConfig _config;

        public CurrenciesPopupController(CurrencyRepositoryService repositoryService, GameObject gameObject, CurrenciesPopupConfig config)
        {
            _repositoryService = repositoryService;
            _gameObject = gameObject;
            _config = config;
        }

        public void Open()
        {
            _gameObject.SetActive(true);
            _repositoryService.UpdatedCurrencyNumber -= UpdateText;
            _repositoryService.UpdatedCurrencyNumber += UpdateText;
            
            UpdateText(_repositoryService.CurrencyNumber);
        }

        public void Close()
        {
            _gameObject.SetActive(false);
            _repositoryService.UpdatedCurrencyNumber -= UpdateText;
        }

        private void UpdateText(int number)
        {
            _config.CurrencyNumberText.text = number.ToString();
        }

        public void Dispose()
        {
            Close();
        }
    }
}