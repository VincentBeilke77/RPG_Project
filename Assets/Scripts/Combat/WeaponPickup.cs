using System;
using System.Collections;
using UnityEngine;

namespace RPGProject.Assets.Scripts.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] private Weapon _weapon = null;
        [SerializeField] private float _respawnTime = 5;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Pick Up Weapon");
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Fighter>().EquipWeapon(_weapon);
                StartCoroutine(HideForSeconds(_respawnTime));
            }
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
    }
}
