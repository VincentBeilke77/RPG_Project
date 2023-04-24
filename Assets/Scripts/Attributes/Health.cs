using RPGProject.Assets.Scripts.Core;
using RPGProject.Assets.Scripts.Saving;
using RPGProject.Assets.Scripts.Stats;
using System;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEngine;

namespace RPGProject.Assets.Scripts.Attributes
{
    public class Health : MonoBehaviour, IJsonSaveable
    {
        
        [SerializeField][Range(0,1)] private float _regenerateHealthPercent = .7f;

        private float _health = -1f;
        private bool _isDead = false;
        private BaseStats _baseStats;

        public bool IsDead { get { return _isDead; } set { _isDead = value; } }

        private void Awake()
        {
            _baseStats = GetComponent<BaseStats>();
            _baseStats.OnLevelUp += RegenerateHealth;
        }
        private void Start()
        {
            if (_health < 0)
            {
                _health = _baseStats.GetStat(Stat.Health);
            }
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            _health = Mathf.Max(_health - damage, 0);

            if (_health == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }

        public float GetPercentage()
        {
            return (_health / _baseStats.GetStat(Stat.Health)) * 100;
        }

        private void AwardExperience(GameObject instigator)
        {
            var experience = instigator.GetComponent<Experience>();
            if (experience == null) return;

            experience.GainExperience(_baseStats.GetStat(Stat.ExperienceReward));
        }

        private void RegenerateHealth()
        {
            float regenHealthPoints = _baseStats.GetStat(Stat.Health) * _regenerateHealthPercent;
            _health = Mathf.Max(_health, regenHealthPoints);
        }

        private void Die()
        {
            if (_isDead) return;

            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
            _isDead = true;
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(_health);
        }

        public void RestoreFromJToken(JToken state)
        {
            _health = state.ToObject<float>();

            if (_health == 0)
            {
                Die();
            }
        }
    }
}
