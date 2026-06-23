using Modules.PlayerUnitModule.Scripts.Merge;
using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Archer.BuyUnit
{
    [CreateAssetMenu(fileName = nameof(BuyMergeUnitConfig), menuName = "Configs/" + nameof(BuyMergeUnitConfig))]
    public class BuyMergeUnitConfig : ScriptableObject
    {
        [field: SerializeField] public int FirstUnitBuyPrice { get; private set; } = 15;
        [field: SerializeField] public int SecondUnitBuyPrice { get; private set; } = 15;
        [field: SerializeField] public int PriceIncrementValueAfterEveryBuy { get; private set; } = 5;
        [field: SerializeField] public MergeUnitComponent SpawnUnitPrefab {get; private set;}
    }
}