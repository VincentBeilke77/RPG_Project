using GameDevTV.Utils;
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

        private LazyValue<float> _healthPoints;
        public float HealthPoints { get { return _healthPoints.value; } }

        private void Awake()
        {
            _baseStats = GetComponent<BaseStats>();
            _healthPoints = new LazyValue<float>(GetInitialHealth);
        }

        private float GetInitialHealth()
        {
            return _baseStats.GetStat(Stat.Health);
        }

        private void Start()
        {
            _healthPoints.ForceInit();
        }

        private void OnEnable()
        {
            _baseStats.OnLevelUp += RegenerateHealth;
        }

        private void OnDisable()
        {
            _baseStats.OnLevelUp -= RegenerateHealth;
        }

        public void TakeDamage(GameObject instigator, float damage)
        {
            print($"{gameObject.name} took damage: {damage}");

            _healthPoints.value = Mathf.Max(_healthPoints.value - damage, 0);

            if (_healthPoints.value == 0)
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
            return (HealthPoints / _baseStats.GetStat(Stat.Health)) * 100;
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
            _healthPoints.value = Mathf.Max(HealthPoints, regenHealthPoints);
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
            return JToken.FromObject(HealthPoints);
        }

        public void RestoreFromJToken(JToken state)
        {
            _healthPoints.value = state.ToObject<float>();

            if (HealthPoints == 0)
            {
                Die();
            }
        }
    }
}
