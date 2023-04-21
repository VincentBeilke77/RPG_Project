using RPGProject.Assets.Scripts.Core;
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

        public float Range { get { return _range; } }
        public float AttackSpeed { get { return _attackSpeed; } }
        public float Damage { get { return _damage; } }

        [SerializeField] private GameObject _equippedPrefab = null;
        [SerializeField] private AnimatorOverrideController _animatorOverride = null;

        public void Spawn(Transform rightHand, Transform leftHand, Animator animator)
        {
            if (_equippedPrefab != null)
            {
                Transform handTransform = GetHandTransform(rightHand, leftHand);

                Instantiate(_equippedPrefab, handTransform);
            }

            if (_animatorOverride != null)
            {
                animator.runtimeAnimatorController = _animatorOverride;
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
    }
}