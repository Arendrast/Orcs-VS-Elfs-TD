using System;
using Modules.PlayerUnitModule.Scripts.Archer.BuyUnit.ShowMoneyPopup;
using Modules.PlayerUnitModule.Scripts.Merge;
using Modules.SharedModule.Scripts.Audio;
using Modules.SharedModule.Scripts.Currencies;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Archer.BuyUnit.BuyUnitPopup
{
    public class BuyUnitPopupComponent : MonoBehaviour
    {
        public BuyUnitPopupController Controller { get; private set; }
        
        [SerializeField] private BuyUnitPopupConfig _config;


        private void Awake()
        {
            _config.BuyButton.gameObject.SetActive(false);
        }

        public void Construct(BuyUnitModel model, MergeUnitFactory mergeUnitFactory,
            BuyMergeUnitConfig buyMergeUnitConfig, AudioService audioService,
            CurrencyRepositoryService currencyRepositoryService)
        {
            Controller = new BuyUnitPopupController(_config, model, mergeUnitFactory, buyMergeUnitConfig, audioService, currencyRepositoryService);
        }
    }
}