using UnityEngine;

namespace RPGProject.Assets.Scripts.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        private void Update()
        {
            if (!GetComponent<ParticleSystem>().IsAlive()) 
            {
                Destroy(gameObject);
            }
        }
    }
}
