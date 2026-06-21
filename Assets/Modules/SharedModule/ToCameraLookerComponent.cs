using UnityEngine;

namespace Modules.SharedModule
{
    public class ToCameraLookerComponent : MonoBehaviour
    {
        [SerializeField] private float _rotationY = 90;
        [SerializeField] private bool _setRotation = true;

        private Camera _camera;

        public void Construct(Camera newCamera)
        {
            _camera = newCamera;
        }

        private void LateUpdate()
        {
            transform.rotation = Quaternion.LookRotation(_camera.transform.position - transform.position);

            if (_setRotation)
            {
                transform.rotation =
                    Quaternion.Euler(transform.rotation.eulerAngles.x, _rotationY, transform.rotation.eulerAngles.z);
            }
        }
    }
}