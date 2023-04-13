using RPGProject.Assets.Scripts.Combat;
using RPGProject.Assets.Scripts.Core;
using RPGProject.Assets.Scripts.Movement;
using UnityEngine;

namespace RPGProject.Assets.Scripts.Controllers
{
    public class AIController : MonoBehaviour
    {
        [SerializeField]
        private float _chaseDistance = 5f;
        [SerializeField]
        private float _suspicionTime = 3f;
        [SerializeField]
        private PathPatrol _pathPatrol;
        [SerializeField]
        private float _waypointTolerance = 1f;
        [SerializeField]
        private float _waypointDwellTime = 3f;
        [Range(0,1)] 
        [SerializeField]
        private float _patrolSpeedFraction = .2f;

        private GameObject _player;
        private Health _health;
        private Mover _mover;
        private Fighter _fighter;
        private ActionScheduler _actionScheduler;

        private Vector3 _guardPosition;
        private float _timeSinceLastSeenPlayer = Mathf.Infinity;
        private float _timeSinceLastWaypoint = Mathf.Infinity;
        private int _currentWaypointIndex = 0;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player");
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
            _fighter = GetComponent<Fighter>();
            _actionScheduler = GetComponent<ActionScheduler>();

            _guardPosition = transform.position;
        }

        private void Update()
        {
            if (_health.IsDead) return;

            if (InAttackRange() && _fighter.CanAttack(_player))
            {
                AttackBehaviour();
            }
            else if (_timeSinceLastSeenPlayer < _suspicionTime)
            {
                SuspicionBehaviour();
            }
            else
            {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        private void UpdateTimers()
        {
            _timeSinceLastSeenPlayer += Time.deltaTime;
            _timeSinceLastWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = _guardPosition;

            if (_pathPatrol != null)
            {
                if (AtWaypoint())
                {
                    _timeSinceLastWaypoint = 0;
                    CycleWaypoint();
                }
                nextPosition = GetCurrentWaypoint();
            }

            if (_timeSinceLastWaypoint > _waypointDwellTime)
            {
                _mover.StartMoveAction(nextPosition, _patrolSpeedFraction);
            }
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < _waypointTolerance;
        }

        private void CycleWaypoint()
        {
            _timeSinceLastWaypoint = 0;
            _currentWaypointIndex = _pathPatrol.GetNextIndex(_currentWaypointIndex);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return _pathPatrol.GetWaypoint(_currentWaypointIndex);
        }

        private void SuspicionBehaviour()
        {
            _actionScheduler.CancelCurrentAction();
        }

        private void AttackBehaviour()
        {
            _timeSinceLastSeenPlayer = 0;
            _fighter.Attack(_player);
        }

        private bool InAttackRange()
        {
            var distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);
            return  distanceToPlayer <= _chaseDistance;
        }

        // called by Unity
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, _chaseDistance);
        }
    }
}
