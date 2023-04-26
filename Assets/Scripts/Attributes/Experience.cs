using RPGProject.Assets.Scripts.Saving;
using System;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace RPGProject.Assets.Scripts.Attributes
{
    public class Experience : MonoBehaviour, IJsonSaveable
    {
        [SerializeField] float _points = 0;
        public float Points { get { return _points; } }

        public event Action OnExperiencedGained;

        public void GainExperience(float experience)
        {
            _points += experience;
            OnExperiencedGained();
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(_points);
        }

        public void RestoreFromJToken(JToken state)
        {
            _points = state.ToObject<float>();
        }
    }
}
