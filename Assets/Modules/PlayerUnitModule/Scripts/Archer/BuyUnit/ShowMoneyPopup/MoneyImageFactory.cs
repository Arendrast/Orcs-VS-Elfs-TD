using Modules.SharedModule.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Modules.PlayerUnitModule.Scripts.Archer.BuyUnit.ShowMoneyPopup
{
    public class MoneyImageFactory
    {
        private readonly Pool<Image> _pool;

        public MoneyImageFactory()
        {
            _pool = new Pool<Image>(null, null, null, createParent: false);
        }

        public Image GetMoneyImage(Image prefab, Vector3 position, Transform parent)
        {
            var image = _pool.TryGet(prefab, parent);
            image.transform.position = position;

            return image;
        }

        public void ReleaseMoneyImage(Image image)
        {
            _pool.TryRelease(image);
        }
    }
}