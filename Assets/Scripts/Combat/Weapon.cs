using RPGProject.Assets.Scripts.Attributes;
using System;
using UnityEngine;

namespace RPGProject.Assets.Scripts.Combat
{
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Make New Weapon", order = 0)]
    public class Weapon : ScriptableObject
    {
        [SerializeField] private bool _isRightHanded = true;
        [SerializeField] private Projectile _projectile = null;

        const string WeaponName = "Weapon";

        [SerializeField] private float _range = 2f;
        public float Range { get { return _range; } }

        [SerializeField] private float _attackSpeed = 1f;
        public float AttackSpeed { get { return _attackSpeed; } }

        [SerializeField] private float _damage = 5f;
        public float Damage { get { return _damage; } }

        [SerializeField] private float _percentageBonus = 0f;
        public float PercentageBonus { get { return _percentageBonus; } }


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

        public void LaunchProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage)
        {
            var projectileInstance = Instantiate(
                _projectile, GetHandTransform(rightHand, leftHand).position, Quaternion.identity);

            projectileInstance.SetTarget(target, instigator, calculatedDamage);
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