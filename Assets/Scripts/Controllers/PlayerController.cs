using RPGProject.Assets.Scripts.Combat;
using RPGProject.Assets.Scripts.Core;
using RPGProject.Assets.Scripts.Movement;
using UnityEngine;

namespace RPGProject.Assets.Scripts.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        private Fighter _fighter;
        private Health _health;
        private Mover _mover;

        private void Start()
        {
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
        }

        // Update is called once per frame
        void Update()
        {
            if (_health.IsDead) return;
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;
        }

        private bool InteractWithCombat()
        {            
            var hits = Physics.RaycastAll(GetMouseRay());
            foreach (var hit in hits)
            {
                var target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (!_fighter.CanAttack(target.gameObject)) continue;

                if (Input.GetMouseButton(0))
                {
                    _fighter.Attack(target.gameObject);
                }
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            return MoveToCursor();
        }

        private bool MoveToCursor()
        {
            if (Physics.Raycast(GetMouseRay(), out RaycastHit hitInfo))
            {
                if (Input.GetMouseButton(0))
                {
                    _mover.StartMoveAction(hitInfo.point);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}