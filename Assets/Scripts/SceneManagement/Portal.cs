using RPGProject.Assets.Scripts.Controllers;
using RPGProject.Assets.Scripts.Movement;
using System;
using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPGProject.Assets.SceneManagement
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
            yield return SceneManager.LoadSceneAsync(_sceneToLoad);

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            var player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().Warp(otherPortal._spawnPoint.position);
            //player.transform.position = otherPortal._spawnPoint.position;
            //player.transform.rotation = otherPortal._spawnPoint.rotation;
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
