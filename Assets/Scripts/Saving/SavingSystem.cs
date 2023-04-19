using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RPGProject.Assets.Scripts.Saving
{
    public class SavingSystem : MonoBehaviour
    {

        public void Save(string saveFileName)
        {
            Dictionary<string,object> state = LoadFile(saveFileName);
            
            CaptureState(state);

            SaveFile(saveFileName, state);
        }

        public void Load(string saveFileName)
        {
            RestoreState(LoadFile(saveFileName));
        }

        public IEnumerator LoadLastScene(string saveFileName)
        {
            Dictionary<string, object> state = LoadFile(saveFileName);

            if (state.ContainsKey("lastSceneBuildIndex"))
            {
                var buildIndex = (int)state["lastSceneBuildIndex"];

                if (buildIndex != SceneManager.GetActiveScene().buildIndex)
                {
                    yield return SceneManager.LoadSceneAsync(buildIndex);
                }
            }

            RestoreState(state);
        }

        private Dictionary<string, object> LoadFile(string saveFileName)
        {
            var path = GetPathFromSaveFile(saveFileName);
            if (!File.Exists(path))
            {
                return new Dictionary<string, object>();
            }

            using (var stream = File.Open(path, FileMode.Open))
            {
                var formatter = new BinaryFormatter();
                return (Dictionary<string, object>)formatter.Deserialize(stream);
            }
        }

        private void SaveFile(string saveFileName, object state)
        {
            var path = GetPathFromSaveFile(saveFileName);
            print($"Saving to {path}");
            using (var stream = File.Open(path, FileMode.Create))
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, state);
            }
        }

        private void CaptureState(Dictionary<string, object> state)
        {
            foreach(SaveableEntity saveable in FindObjectsOfType<SaveableEntity>()) 
            {
                state[saveable.UniqueIdentifier] = saveable.CaptureState();
            }

            state["lastSceneBuildIndex"] = SceneManager.GetActiveScene().buildIndex;
        }

        private void RestoreState(Dictionary<string, object> state)
        {
            foreach (SaveableEntity saveable in FindObjectsOfType<SaveableEntity>())
            {
                string uniqueIdentifier = saveable.UniqueIdentifier;
                if (state.ContainsKey(uniqueIdentifier))
                {
                    saveable.RestoreState(state[uniqueIdentifier]);
                }
            }
        }

        private string GetPathFromSaveFile(string saveFile)
        {
            
            return Path.Combine(Application.persistentDataPath, saveFile + ".sav");
        }
    }
}
