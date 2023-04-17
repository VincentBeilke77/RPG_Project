using System;
using UnityEngine;

namespace RPGProject.Assets.Scripts.Core
{
    public class PersistantObjectSpawner :MonoBehaviour
    {
        [SerializeField] private GameObject _persistantObjectPrefab;

        public static bool hasSpawned = false;

        private void Awake()
        {
            if (hasSpawned) return;

            SpawnPersistantOjbects();

            hasSpawned = true;
        }

        private void SpawnPersistantOjbects()
        {
            var persistantObject = Instantiate(_persistantObjectPrefab);
            DontDestroyOnLoad(persistantObject);
        }
    }
}
