using RPGProject.Assets.Scripts.Attributes;
using UnityEngine;
using UnityEngine.Events;

namespace RPGProject.Assets.Scripts.Combat
{
    public class Projectile : MonoBehaviour
    {        
        [SerializeField] private float _speed = 1f;
        [SerializeField] private bool _isHoming = false;
        [SerializeField] private GameObject _hitEffect = null;
        [SerializeField] private float _maxLifeTime = 5f;
        [SerializeField] GameObject[] _destroyOnHit = null;
        [SerializeField] private float _lifeAfterImpact = 2f;

        [SerializeField] private UnityEvent OnHt;

        private Health _target = null;
        public Health Target { set { _target = value; } }

        private float _damage = 0f;
        private GameObject _instigator = null;

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

        public void SetTarget(Health target, GameObject instigator, float damage)
        {
            Target = target;
            _damage = damage;
            _instigator = instigator;

            Destroy(gameObject, _maxLifeTime);
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

            _target.TakeDamage(_instigator, _damage);

            _speed = 0f;

            OnHt.Invoke();

            if (_hitEffect != null)
            {
                Instantiate(_hitEffect, GetAimLocation(), transform.rotation);
            }

            foreach(GameObject toDestroy in _destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, _lifeAfterImpact);
        }
    }
}
