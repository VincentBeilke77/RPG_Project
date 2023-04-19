using RPGProject.Assets.Scripts.Controllers;
using RPGProject.Assets.Scripts.Movement;
using RPGProject.Assets.Scripts.Saving;
using RPGProject.Assets.Scripts.SceneManagement;
using System;
using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPGProject.Assets.Scripts.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        private enum DestinationIdentifier
        {
            A, B, C, D, E
        }

        [SerializeField] private int _sceneToLoad = -1;
        [SerializeField] private Transform _spawnPoint;
        [SerializeField] private DestinationIdentifier _destination;
        [SerializeField] private float _fadeOutTime = .5f;
        [SerializeField] private float _fadeInTime = 1f;
        [SerializeField] private float _fadeWaitTime = .5f;
        private void Start()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            if (_sceneToLoad < 0)
            {
                Debug.LogError("Scened to load not set.");
                yield break;
            }

            DontDestroyOnLoad(gameObject);

            var fader = FindObjectOfType<Fader>();
            var savingWrapper = FindObjectOfType<SavingWrapper>();

            yield return fader.FadeOut(_fadeOutTime);

            savingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(_sceneToLoad);

            savingWrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            savingWrapper.Save();

            yield return new WaitForSeconds(_fadeWaitTime);

            yield return fader.FadeIn(_fadeInTime);

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            var player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal._spawnPoint.position);
        }

        private Portal GetOtherPortal()
        {
            foreach (var portal in FindObjectsOfType<Portal>()) {
                if (portal == this) continue;
                if (portal._destination != _destination) continue;

                return portal;
            }

            return null;
        }
    }
}
