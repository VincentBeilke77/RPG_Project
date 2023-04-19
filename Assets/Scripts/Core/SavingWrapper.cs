using RPGProject.Assets.Scripts.SceneManagement;
using System.Collections;
using UnityEngine;

namespace RPGProject.Assets.Scripts.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        private JsonSavingSystem _savingSystem;

        private const string DefaultSaveFile = "save";
        private Fader _fader;

        [SerializeField] float fadeInTime = 1f;

        private void Awake()
        {
            _fader = FindObjectOfType<Fader>();
            _savingSystem = GetComponent<JsonSavingSystem>();
        }

        private IEnumerator Start()
        {
            _fader.FadeOutImmediate();
            yield return _savingSystem.LoadLastScene(DefaultSaveFile);
            yield return _fader.FadeIn(fadeInTime);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.S)) 
            {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.L)) 
            {
                Load();
            }
        }

        public void Save()
        {
            _savingSystem.Save(DefaultSaveFile);
        }

        public void Load()
        {
            _savingSystem.Load(DefaultSaveFile);
        }
    }
}
