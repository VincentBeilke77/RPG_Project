using Codice.CM.Triggers;
using RPGProject.Assets.Scripts.Core;
using System;
using UnityEngine;

namespace RPGProject.Assets.Scripts.Combat
{
    public class Projectile : MonoBehaviour
    {        
        [SerializeField] private float _speed = 1f;
        [SerializeField] private bool _isHoming = false;
        [SerializeField] private GameObject _hitEffect = null;

        private Health _target = null;
        public Health Target { set { _target = value; } }

        private float _damage = 0f;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        private void Update()
        {
            if (_target == null) return;

            if (_isHoming && !_target.IsDead) transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * _speed* Time.deltaTime);
        }

        public void SetTarget(Health target, float damage)
        {
            Target = target;
            _damage = damage;
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider targetCapsule = _target.GetComponent<CapsuleCollider>();
            
            if (targetCapsule == null)
            {
                return _target.transform.position;
            }

            return _target.transform.position + (Vector3.up * ((targetCapsule.height / 3) * 2));
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != _target) return;
            
            if (_target.IsDead) return;

            _target.TakeDamage(_damage);

            if (_hitEffect != null)
            {
                Instantiate(_hitEffect, GetAimLocation(), transform.rotation);
            }

            Destroy(gameObject);
        }
    }
}
