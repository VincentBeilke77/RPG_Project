﻿using UnityEngine;

namespace RPGProject.Assets.Scripts.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField] private GameObject _targetToDestroy = null;
        private void Update()
        {
            if (!GetComponent<ParticleSystem>().IsAlive()) 
            {
                if (_targetToDestroy != null)
                {
                    Destroy(_targetToDestroy);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}
