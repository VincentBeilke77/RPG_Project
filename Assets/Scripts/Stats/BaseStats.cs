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

        public int CalculateLevel()
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
