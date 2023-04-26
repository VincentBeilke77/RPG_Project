using GameDevTV.Utils;
using RPGProject.Assets.Scripts.Attributes;
using RPGProject.Assets.Scripts.Core;
using RPGProject.Assets.Scripts.Movement;
using RPGProject.Assets.Scripts.Saving;
using RPGProject.Assets.Scripts.Stats;
using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace RPGProject.Assets.Scripts.Combat
{
    public class Fighter : MonoBehaviour, IAction, IJsonSaveable, IModifierProvider
    {
        [SerializeField] private Transform _rightHandTransform = null;
        [SerializeField] private Transform _leftHandTransform = null;
        [SerializeField] private Weapon _defaultWeapon = null;

        private Mover _mover;
        private Animator _animator;
        private ActionScheduler _actionScheduler;
        private BaseStats _stats;

        private float _attackTime = Mathf.Infinity;
        private LazyValue<Weapon> _currentWeapon;

        private Health _target;
        public Health Target { get { return _target; } }

        private void Awake()
        {
            _mover = GetComponent<Mover>();
            _animator = GetComponent<Animator>();
            _actionScheduler = GetComponent<ActionScheduler>();
            _stats = GetComponent<BaseStats>();
            _currentWeapon = new LazyValue<Weapon>(SetupDefaultWeapon);
        }

        private Weapon SetupDefaultWeapon()
        {
            AttachWeapon(_defaultWeapon);
            return _defaultWeapon;
        }

        private void Start()
        {
            _currentWeapon.ForceInit();
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
            _currentWeapon.value = weapon;
            AttachWeapon(weapon);
        }

        private void AttachWeapon(Weapon weapon)
        {
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

            if (_attackTime > _currentWeapon.value.AttackSpeed)
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
            float damage =  _stats.GetStat(Stat.Damage);
            if (_currentWeapon.value.HasProjectile())
            {
                _currentWeapon.value.LaunchProjectile(_rightHandTransform, _leftHandTransform, _target, gameObject, damage);
            }
            else
            {
                _target.TakeDamage(gameObject, damage);
            }
        }

        // Animation Event
        void Shoot()
        {
            Hit();
        }

        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, _target.transform.position) <= _currentWeapon.value.Range;
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

        public IEnumerable<float> GetAdditiveModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return _currentWeapon.value.Damage;
            }
        }

        public JToken CaptureAsJToken()
        {
            return JToken.FromObject(_currentWeapon.value.name);
        }

        public void RestoreFromJToken(JToken state)
        {
            string weaponName = state.ToObject<string>();
            Weapon weapon = Resources.Load<Weapon>(weaponName);
            EquipWeapon(weapon);
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat)
        {
            if (stat == Stat.Damage)
            {
                yield return _currentWeapon.value.PercentageBonus;
            }
        }
    }
}
