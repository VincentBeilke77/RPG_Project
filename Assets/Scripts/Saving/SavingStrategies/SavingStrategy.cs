﻿using System.IO;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace RPGProject.Assets.Scripts.Saving.SavingStrategies
{
    public abstract class SavingStrategy : ScriptableObject
    {
        public abstract void SaveToFile(string saveFile, JObject state);

        public abstract JObject LoadFromFile(string saveFile);
        public abstract string GetExtension();

        public string GetPathFromSaveFile(string saveFile)
        {
            return Path.Combine(Application.persistentDataPath, saveFile + GetExtension());
        }
    }
}
