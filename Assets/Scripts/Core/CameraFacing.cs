using UnityEngine;

namespace RPGProject.Assets.Scripts.Core
{
    public class CameraFacing : MonoBehaviour
    {
        void Update()
        {
            transform.forward = Camera.main.transform.forward;
        }
    }
}
