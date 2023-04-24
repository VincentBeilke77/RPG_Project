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

        private BaseStats _baseStats;

        private bool _isDead = false;
        public bool IsDead { get { return _isDead; } set { _isDead = value; } }

        private float _healthPoints = -1f;
        public float HealthPoints { get { return _healthPoints; } }

        private void Awake()
        {
            _baseStats = GetComponent<BaseStats>();
            _baseStats.OnLevelUp += RegenerateHealth;
        }
        private void Start()
        {
            if (_healthPoints < 0)
            {
                _healthPoints = _baseStats.GetStat(Stat.Health);
            }
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print($"{gameObject.name} took damage: {damage}");

            _healthPoints = Mathf.Max(_healthPoints - damage, 0);

            if (_healthPoints == 0)
            {
                Die();
                AwardExperience(instigator);
            }
        }        

        public float GetMaxHealthPoints()
        {
            return _baseStats.GetStat(Stat.Health);
        }

        public float GetPercentage()
        {
            return (_healthPoints / _baseStats.GetStat(Stat.Health)) * 100;
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
            _healthPoints = Mathf.Max(_healthPoints, regenHealthPoints);
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
            return JToken.FromObject(_healthPoints);
        }

        public void RestoreFromJToken(JToken state)
        {
            _healthPoints = state.ToObject<float>();

            if (_healthPoints == 0)
            {
                Die();
            }
        }
    }
}
