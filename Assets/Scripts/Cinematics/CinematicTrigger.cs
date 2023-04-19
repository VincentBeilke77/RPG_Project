using RPGProject.Assets.Scripts.Saving;
using System;
using UnityEngine;
using UnityEngine.Playables;

namespace RPGProject.Assets.Scripts.Cinematics
{
    public class CinematicTrigger : MonoBehaviour, ISaveable
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

        public object CaptureState()
        {
            return _triggered;
        }

        public void RestoreState(object state)
        {
            _triggered = (bool)state;
        }
    }
}
