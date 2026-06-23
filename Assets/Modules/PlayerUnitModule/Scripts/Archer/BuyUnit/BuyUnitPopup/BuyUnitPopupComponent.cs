using System;
using Modules.PlayerUnitModule.Scripts.Merge;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Archer.BuyUnit.BuyUnitPopup
{
    public class BuyUnitPopupComponent : MonoBehaviour
    {
        [SerializeField] private BuyUnitPopupConfig _config;

        private void Awake()
        {
            _config.BuyButton.gameObject.SetActive(false);
        }

        public void Construct(BuyUnitModel model, MergeUnitFactory mergeUnitFactory,
            BuyMergeUnitConfig buyMergeUnitConfig)
        {
            new BuyUnitPopupController(_config, model, mergeUnitFactory, buyMergeUnitConfig);
        }
    }
}