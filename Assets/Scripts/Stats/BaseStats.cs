using RPGProject.Assets.Scripts.Attributes;
using System;
using UnityEngine;

namespace RPGProject.Assets.Scripts.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] private int _startingLevel;
        [SerializeField] private CharacterClass _characterClass;
        [SerializeField] private Progression _progression = null;
        [SerializeField] private GameObject _levelUpParticleEffect = null;
        [SerializeField] private bool _canUseModifiers = false;

        public event Action OnLevelUp;

        private Experience _experience;
        private int _currentLevel = 0;

        private void Start()
        {
            _currentLevel = CalculateLevel();
            if (_experience != null)
            {
                _experience.OnExperiencedGained += UpdateLevel;
            }
        }

        private void Awake()
        {
            _experience = GetComponent<Experience>();
        }

        private void UpdateLevel()
        {
            int newLevel = CalculateLevel();
            if (newLevel > _currentLevel) 
            {
                _currentLevel = newLevel;
                LevelUpEffect();
                OnLevelUp();
            }
        }

        public float GetStat(Stat stat)
        {
            return (GetBaseStat(stat) + GetAdditiveModifier(stat)) * (1 + (GetPercentageModifier(stat) / 100));
        }

        private float GetBaseStat(Stat stat)
        {
            return _progression.GetStat(stat, _characterClass, GetLevel());
        }

        public int GetLevel()
        {
            if (_currentLevel < 1)
            {
                _currentLevel = CalculateLevel();
            }

            return _currentLevel;
        }

        private float GetAdditiveModifier(Stat stat)
        {
            if (!_canUseModifiers) return 0f;

            var total = 0f;
            foreach (var providor in GetComponents<IModifierProvider>())
            {
                foreach (var modifier in providor.GetAdditiveModifiers(stat))
                {
                    total += modifier;
                }
            }
            return total;
        }

        private float GetPercentageModifier(Stat stat)
        {
            if (!_canUseModifiers) return 0f;

            var total = 0f;
            foreach (var providor in GetComponents<IModifierProvider>())
            {
                foreach (var modifier in providor.GetPercentageModifiers(stat))
                {
                    total += modifier;
                }
            }

            return total;
        }

        private int CalculateLevel()
        {
            if (_experience == null) return _startingLevel;

            var currentXP = _experience.Points;
            var penultimateLevel = _progression.GetLevels(Stat.LevelUpExperience, _characterClass);
            for (int level = 1; level <= penultimateLevel; level++)
            {
                float levelUpXP = _progression.GetStat(Stat.LevelUpExperience, _characterClass, level);
                if (levelUpXP > currentXP)
                {
                    return level;
                }
            }

            return penultimateLevel + 1;
        }

        private void LevelUpEffect()
        {
            Instantiate(_levelUpParticleEffect, transform);
        }
    }
}
