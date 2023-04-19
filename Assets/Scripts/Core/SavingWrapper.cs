using RPGProject.Assets.Scripts.SceneManagement;
using System.Collections;
using UnityEngine;

namespace RPGProject.Assets.Scripts.Saving
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string DefaultSaveFile = "save";
        private Fader _fader;

        [SerializeField] float fadeInTime = 1f;

        private void Awake()
        {
            _fader = FindObjectOfType<Fader>();
        }

        private IEnumerator Start()
        {
            _fader.FadeOutImmediate();
            yield return GetComponent<SavingSystem>().LoadLastScene(DefaultSaveFile);
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
            GetComponent<SavingSystem>().Save(DefaultSaveFile);
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(DefaultSaveFile);
        }
    }
}
