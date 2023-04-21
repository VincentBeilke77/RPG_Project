using RPGProject.Assets.Scripts.Core;
using RPGProject.Assets.Scripts.Saving;
using System.Collections.Generic;
using Unity.Plastic.Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.AI;

namespace RPGProject.Assets.Scripts.Movement
{
    public class Mover : MonoBehaviour, IAction, ISaveable, IJsonSaveable
    {
        [SerializeField] 
        private Camera _cam;
        [SerializeField]
        private float _maxSpeed = 6f;

        private NavMeshAgent _agent;
        private Health _health;
        private ActionScheduler _actionScheduler;

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();  
            _health = GetComponent<Health>();
            _actionScheduler = GetComponent<ActionScheduler>();
        }

        // Update is called once per frame
        void Update()
        {
            _agent.enabled = !_health.IsDead;
            UpdateAnimator();
        }

        public void StartMoveAction(Vector3 destination, float speedFraction)
        {
            _actionScheduler.StartAction(this);
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

        public object CaptureState()
        {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state)
        {
            var position = (SerializableVector3)state;
            _agent.Warp(position.ToVector());
            _actionScheduler.CancelCurrentAction();
        }

        public JToken CaptureAsJToken()
        {
            return transform.position.ToToken();
        }

        public void RestoreFromJToken(JToken state)
        {
            _agent.Warp(state.ToVector3());
            _actionScheduler.CancelCurrentAction();
            
        }
    }
}