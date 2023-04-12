using RPGProject.Assets.Scripts.Core;
using RPGProject.Assets.Scripts.Movement;
using UnityEngine;

namespace RPGProject.Assets.Scripts.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField]
        private float _weaponRange = 2f;
        [SerializeField]
        private float _attackSpeed = 1f;
        [SerializeField]
        private float _weaponDamage = 5f;

        private Health _target;
        private Mover _mover;
        private Animator _animator;
        private ActionScheduler _actionScheduler;

        private float _attackTime = Mathf.Infinity;


        private void Start()
        {
            _mover = GetComponent<Mover>();
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Update()
        {
            if (_target == null) return;
            if (_target.IsDead) return;

            _attackTime += Time.deltaTime;

            if (!GetIsInRange())
            {
                _mover.MoveTo(_target.transform.position);
            }
            else
            {
                _mover.Cancel();        
                AttackBehaviour();
            }
        }

        public bool CanAttack(GameObject combatTarget)
        {
            if (combatTarget == null) return false;
            var targetToTest = combatTarget.GetComponent<Health>();
            return targetToTest != null && !targetToTest.IsDead;
        }

        public void Attack(GameObject combatTarget)
        {
            _actionScheduler.StartAction(this);
            _target = combatTarget.GetComponent<Health>();
        }

        private void AttackBehaviour()
        {
            transform.LookAt(_target.transform);

            if (_attackTime > _attackSpeed)
            {
                TriggerAttack();
                _attackTime = 0;
            }
        }

        private void TriggerAttack()
        {
            // This will trigger Hit() event.
            _animator.ResetTrigger("stopAttack");
            _animator.SetTrigger("attack");
        }

        // Animation Event
        void Hit()
        {
            if (_target == null) return;
            _target.TakeDamage(_weaponDamage);
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, _target.transform.position) <= _weaponRange;
        }

        public void Cancel()
        {
            StopAttack();
            _target = null;
        }

        private void StopAttack()
        {
            _animator.ResetTrigger("attack");
            _animator.SetTrigger("stopAttack");
        }
    }
}
