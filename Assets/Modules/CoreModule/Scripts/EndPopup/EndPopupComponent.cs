using UnityEngine;

namespace Modules.CoreModule.Scripts.EndPopup
{
    public class EndPopupComponent : MonoBehaviour
    {
        public EndPopupController Controller { get; private set; }
        
        [SerializeField] private EndPopupConfig _config;

        public void Awake()
        {
            gameObject.SetActive(false);
            Controller = new EndPopupController(_config, gameObject);
        }

        private void OnDisable()
        {
            Controller?.Dispose();
        }
    }
}