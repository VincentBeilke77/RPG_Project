using RPGProject.Assets.Scripts.Core;
using UnityEngine;
using UnityEngine.AI;

namespace RPGProject.Assets.Scripts.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        [SerializeField] 
        private Camera _cam;
        [SerializeField]
        private float _maxSpeed = 6f;

        private NavMeshAgent _agent;
        private Health _health;

        private void Start()
        {
            _agent = GetComponent<NavMeshAgent>();  
            _health = GetComponent<Health>();
        }

        // Update is called once per frame
        void Update()
        {
            _agent.enabled = !_health.IsDead;
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction)
        {
            _agent.destination = destination;
            _agent.isStopped = false;
            _agent.speed = _maxSpeed * Mathf.Clamp01(speedFraction);
        }

        public void Cancel()
        {
            _agent.isStopped = true;
        }

        private void UpdateAnimator()
        {
            var velocity = _agent.velocity;
            var localVelocity = transform.InverseTransformDirection(velocity);

            var speed = localVelocity.z;

            var animator = GetComponent<Animator>();
            animator.SetFloat("forwardSpeed", speed);
        }


    }
}