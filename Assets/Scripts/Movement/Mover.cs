using RPGProject.Assets.Scripts.Attributes;
using RPGProject.Assets.Scripts.Core;
using RPGProject.Assets.Scripts.Saving;
using Newtonsoft.Json.Linq;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.GraphicsBuffer;

namespace RPGProject.Assets.Scripts.Movement
{
    public class Mover : MonoBehaviour, IAction, IJsonSaveable
    {
        [SerializeField] private Camera _cam;
        [SerializeField] private float _maxSpeed = 6f;
        [SerializeField] private float _maxNavPathLength = 40.0f;

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

        public bool CanMoveTo(Vector3 destination)
        {
            NavMeshPath path = new NavMeshPath();
            var hasPath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, path);
            if (!hasPath) return false;
            if (path.status != NavMeshPathStatus.PathComplete) return false;
            if (GetPathLength(path) > _maxNavPathLength) return false;

            return true;
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

        private float GetPathLength(NavMeshPath path)
        {
            float pathLength = 0;

            if (path.corners.Length < 2) return pathLength;

            for (int i = 0; i < path.corners.Length - 1; i++)
            {
                var distance = Vector3.Distance(path.corners[i], path.corners[i + 1]);
                pathLength += distance;
            }

            return pathLength;
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