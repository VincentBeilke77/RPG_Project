using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEditor;
using UnityEngine;

namespace RPGProject.Assets.Scripts.Saving
{
    [ExecuteAlways]
    public class JsonSaveableEntity : MonoBehaviour
    {
        [SerializeField] private string _uniqueIdentifier = "";

        static Dictionary<string, JsonSaveableEntity> globalLookup =
            new Dictionary<string, JsonSaveableEntity>();

        public string UniqueIdentifier { get { return _uniqueIdentifier; } }

        public JToken CaptureAsJToken()
        {
            JObject state = new JObject();

            IDictionary<string, JToken> stateDict = state;

            foreach (var jsonSaveable in GetComponents<IJsonSaveable>())
            {
                JToken token = jsonSaveable.CaptureAsJToken();
                var component = jsonSaveable.GetType().ToString();
                Debug.Log($"{name} Capture {component} = {token}");
                stateDict[component] = token;
            }

            return state;
        }

        public void RestoreFromJToken(JToken state)
        {
            JObject objectState = state.ToObject<JObject>();
            IDictionary<string, JToken> stateDict = objectState;

            foreach (var jsonSaveable in GetComponents<IJsonSaveable>())
            {
                string component = jsonSaveable.GetType().ToString();

                if(stateDict.ContainsKey(component))
                {
                    Debug.Log($"{name} Restore {component} =>{stateDict[component]}");
                    jsonSaveable.RestoreFromJToken(stateDict[component]);
                }
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return;

            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("_uniqueIdentifier");

            if (string.IsNullOrEmpty(property.stringValue) || !IsUnique(property.stringValue))
            {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            globalLookup[property.stringValue] = this;
        }
#endif

        private bool IsUnique(string candidate)
        {
            if (!globalLookup.ContainsKey(candidate)) return true;

            if (globalLookup[candidate] == this) return true;

            if (globalLookup[candidate] == null)
            {
                globalLookup.Remove(candidate);
                return true;
            }

            if (globalLookup[candidate].UniqueIdentifier != candidate)
            {
                globalLookup.Remove(candidate);
                return true;
            }

            return false;

        }
    }
}
