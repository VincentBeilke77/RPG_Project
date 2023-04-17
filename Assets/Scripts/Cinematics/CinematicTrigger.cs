using UnityEngine;
using UnityEngine.Playables;

namespace RPGProject.Assets.Cinematics
{
    public class CinematicTrigger : MonoBehaviour
    {
        private bool _triggered = false;

        private void OnTriggerEnter(Collider other)
        {
            if (!_triggered && other.CompareTag("Player"))
            {
                _triggered = true;
                GetComponent<PlayableDirector>().Play();
            }
        }
    }
}
