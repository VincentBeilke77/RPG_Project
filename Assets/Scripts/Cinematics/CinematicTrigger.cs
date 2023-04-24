using RPGProject.Assets.Scripts.Core;
using RPGProject.Assets.Scripts.Saving;
using System;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.Playables;

namespace RPGProject.Assets.Scripts.Cinematics
{
    public class CinematicTrigger : MonoBehaviour, IJsonSaveable
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

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(_triggered);
        }

        public void RestoreFromJToken(JToken state)
        {
            _triggered = state.ToObject<bool>();
        }
    }
}
