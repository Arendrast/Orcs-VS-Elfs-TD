using UnityEngine;

namespace Modules.PlayerUnitModule.Scripts.Merge.MergeUnitTutorialPopup
{
    public class MergeUnitTutorialPopupComponent : MonoBehaviour
    {
        public MergeUnitTutorialPopupController PopupController { get; private set; }
        
        [field: SerializeField] public MergeUnitTutorialPopupConfig Config { get; private set; }

        private void Awake()
        {
            gameObject.SetActive(false);
            PopupController = new MergeUnitTutorialPopupController(Config, gameObject);
        }

        private void OnDisable()
        {
            PopupController?.Dispose();
        }
    }
}