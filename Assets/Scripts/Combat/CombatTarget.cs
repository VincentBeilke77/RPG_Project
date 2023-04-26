using RPGProject.Assets.Scripts.Attributes;
using RPGProject.Assets.Scripts.Controllers;
using UnityEngine;

namespace RPGProject.Assets.Scripts.Combat
{
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable
    {
        public bool HandleRaycast(PlayerController callingController)
        {
            var fighter = callingController.GetComponent<Fighter>();

            if (!fighter.CanAttack(gameObject)) return false;

            if (Input.GetMouseButton(0)) fighter.Attack(gameObject);

            return true;
        }

        public CursorType GetCursorType()
        {
            return CursorType.Combat;
        }
    }
}
