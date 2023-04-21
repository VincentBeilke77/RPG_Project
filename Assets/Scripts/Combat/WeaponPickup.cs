using UnityEngine;

namespace RPGProject.Assets.Scripts.Combat
{
    public class WeaponPickup : MonoBehaviour
    {
        [SerializeField] private Weapon _weapon = null;

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Pick Up Weapon");
            if (other.CompareTag("Player"))
            {
                other.GetComponent<Fighter>().EquipWeapon(_weapon);
                Destroy(gameObject);
            }
        }
    }
}
