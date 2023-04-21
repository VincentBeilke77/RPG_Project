using RPGProject.Assets.Scripts.Core;
using System;
using UnityEngine;

namespace RPGProject.Assets.Scripts.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private float _range = 2f;
        [SerializeField] private float _attackSpeed = 1f;
        [SerializeField] private float _damage = 5f;
        [SerializeField] private bool _isRightHanded = true;
        [SerializeField] private Projectile _projectile = null;

        const string WeaponName = "Weapon";

        public float Range { get { return _range; } }
        public float AttackSpeed { get { return _attackSpeed; } }
        public float Damage { get { return _damage; } }

        [SerializeField] private GameObject _equippedPrefab = null;
        [SerializeField] private AnimatorOverrideController _animatorOverride = null;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            DestroyOldWeapon(rightHand, leftHand);

            if (_equippedPrefab != null)
            {
                Transform handTransform = GetHandTransform(rightHand, leftHand);

                var weapon = Instantiate(_equippedPrefab, handTransform);
                weapon.name = WeaponName;
            }

            var overrideController = animator.runtimeAnimatorController as AnimatorOverrideController;

            if (_animatorOverride != null)
            {
                animator.runtimeAnimatorController = _animatorOverride;
            }
            else if (overrideController != null)
            {
                animator.runtimeAnimatorController = overrideController.runtimeAnimatorController;
            }
            
        }

        public bool HasProjectile()
        {
            return _projectile != null;
        }

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target)
        {
            var projectileInstance = Instantiate(
                _projectile, GetHandTransform(rightHand, leftHand).position, Quaternion.identity);

            projectileInstance.SetTarget(target, Damage);
        }

        private Transform GetHandTransform(Transform rightHand, Transform leftHand)
        {
            return _isRightHanded ? rightHand.transform : leftHand.transform;
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand)
        {
            var oldWeapon = rightHand.Find(WeaponName);
            if (oldWeapon == null) oldWeapon = leftHand.Find(WeaponName);
            if (oldWeapon == null) return;

            oldWeapon.name = "DESTROY";
            Destroy(oldWeapon.gameObject);
        }
    }
}