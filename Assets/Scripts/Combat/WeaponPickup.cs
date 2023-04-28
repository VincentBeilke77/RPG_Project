using RPGProject.Assets.Scripts.Attributes;
using RPGProject.Assets.Scripts.Controllers;
using System;
using System.Collections;
using UnityEngine;

namespace RPGProject.Assets.Scripts.Combat
{
    public class WeaponPickup : MonoBehaviour, IRaycastable
    {
        [SerializeField] private WeaponConfig _weapon = null;
        [SerializeField] private float _healthRestore = 0;
        [SerializeField] private float _respawnTime = 5;

        public bool HandleRaycast(PlayerController callingController)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Pickup(callingController.gameObject);
            }

            return true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                Pickup(other.gameObject);
            }
        }

        private void Pickup(GameObject subject)
        {
            if (_weapon != null)
            {
                subject.GetComponent<Fighter>().EquipWeapon(_weapon);
            }

            if (_healthRestore > 0)
            {
                subject.GetComponent<Health>().Heal(_healthRestore);
            }
            StartCoroutine(HideForSeconds(_respawnTime));
        }

        private IEnumerator HideForSeconds(float seconds)
        {
            ShowPickup(false);
            yield return new WaitForSeconds(seconds);
            ShowPickup(true);
        }

        private void ShowPickup(bool hide)
        {
            GetComponent<Collider>().enabled = hide;
            foreach (Transform child in transform)
            {
                child.gameObject.SetActive(hide);
            }
        }

        public CursorType GetCursorType()
        {
            return CursorType.Pickup;
        }
    }
}
