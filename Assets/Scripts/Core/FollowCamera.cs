using UnityEngine;

namespace RPGProject.Assets.Scripts.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField]
        private Transform _playerTransform;

        // Update is called once per frame
        void LateUpdate()
        {
            transform.position = _playerTransform.position;
        }
    }
}