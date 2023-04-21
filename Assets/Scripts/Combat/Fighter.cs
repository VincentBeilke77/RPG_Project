using RPGProject.Assets.Scripts.Core;
using RPGProject.Assets.Scripts.Movement;
using UnityEngine;

namespace RPGProject.Assets.Scripts.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] private Transform _rightHandTransform = null;
        [SerializeField] private Transform _leftHandTransform = null;
        [SerializeField] private Weapon _defaultWeapon = null;

        private Health _target;
        private Mover _mover;
        private Animator _animator;
        private ActionScheduler _actionScheduler;

        private float _attackTime = Mathf.Infinity;
        private Weapon _currentWeapon = null;

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
        }

        private void Start()
        {
            EquipWeapon(_defaultWeapon);
        }

        private void Update()
        {
            if (_target == null) return;
            if (_target.IsDead) return;

            _attackTime += Time.deltaTime;

            if (!GetIsInRange())
            {
                _mover.MoveTo(_target.transform.position, 1f);
            }
            else
            {
                _mover.Cancel();        
                AttackBehaviour();
            }
        }

        public void EquipWeapon(Weapon weapon)
        {
            _currentWeapon = weapon;
            Animator animator = GetComponent<Animator>();
            weapon.Spawn(_rightHandTransform, _leftHandTransform, animator);
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

            if (_attackTime > _currentWeapon.AttackSpeed)
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
            if (_currentWeapon.HasProjectile())
            {
                _currentWeapon.LaunchProjectile(_rightHandTransform, _leftHandTransform, _target);
            }
            else
            {
                _target.TakeDamage(_currentWeapon.Damage);
            }
        }

        // Animation Event
        void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, _target.transform.position) <= _currentWeapon.Range;
        }

        public void Cancel()
        {
            StopAttack();
            _target = null;
            _mover.Cancel();
        }

        private void StopAttack()
        {
            _animator.ResetTrigger("attack");
            _animator.SetTrigger("stopAttack");
        }
    }
}
