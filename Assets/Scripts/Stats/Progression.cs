using RPGProject.Assets.Scripts.Stats;
using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Progression", menuName = "Stats/New Progression", order = 0)]
public class Progression : ScriptableObject
{
    [SerializeField] private ProgressionCharacterClass[] _characterClasses = null;

    Dictionary<CharacterClass, Dictionary<Stat, float[]>> lookupTable = null;

    public float GetStat(Stat stat, CharacterClass characterClass, int level)
    {
        BuildLookup();

        var levels = lookupTable[characterClass][stat];

        if (levels.Length < level) return 0;

        return levels[level - 1];
    }

    public int GetLevels(Stat stat, CharacterClass characterClass)
    {
        BuildLookup();

        var levels = lookupTable[characterClass][stat];
        return levels.Length;
    }

    private void BuildLookup()
    {
        if (lookupTable != null) return;

        lookupTable = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();

        foreach (var progressionClass in _characterClasses)
        {
            var statLookupTable = new Dictionary<Stat, float[]>();

            foreach (var progressionStat in progressionClass.Stats)
            {
                statLookupTable[progressionStat.Stat] = progressionStat.Levels;
            }

            lookupTable[progressionClass.CharacterClass] = statLookupTable;
        }
    }
}

[Serializable]
internal class ProgressionCharacterClass
{
    public CharacterClass CharacterClass;
    public ProgressionStat[] Stats;
}

[Serializable]
internal class ProgressionStat
{
    public Stat Stat;
    public float[] Levels;
}
