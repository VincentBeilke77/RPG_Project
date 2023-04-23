﻿using UnityEngine;

namespace RPGProject.Assets.Scripts.Stats
{
    public class BaseStats : MonoBehaviour
    {
        [Range(1, 99)]
        [SerializeField] private int _startingLevel;
        [SerializeField] private CharacterClass _characterClass;
        [SerializeField] private Progression _progression = null;

        public float GetHealth()
        {
            return _progression.GetHealth(_characterClass, _startingLevel);
        }
    }
}
