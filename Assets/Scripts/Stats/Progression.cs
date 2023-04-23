using RPGProject.Assets.Scripts.Stats;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
public class Progression : ScriptableObject
{
    [SerializeField] private ProgressionCharacterClass[] _characterClasses = null;

    public float GetHealth(CharacterClass characterClass, int level)
    {
        foreach (var progressionClass in _characterClasses)
        {
            if (progressionClass.characterClass == characterClass)
            {
                return progressionClass.health[level - 1];
            }
        }

        return 0;
    }

    [Serializable]
    internal class ProgressionCharacterClass
    {
        public CharacterClass characterClass;
        public float[] health;
    }
}
