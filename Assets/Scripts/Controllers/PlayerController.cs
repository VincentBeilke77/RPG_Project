using RPGProject.Assets.Scripts.Attributes;
using RPGProject.Assets.Scripts.Movement;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPGProject.Assets.Scripts.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        private Health _health;
        private Mover _mover;

        [SerializeField] private CursorMapping[] _cursorMappings = null;
        [SerializeField] private float _maxNavMeshProjection = 1.0f;

        private void Awake()
        {
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
        }

        // Update is called once per frame
        void Update()
        {
            if (InteractWithUI())
            {
                return;
            }

            if (_health.IsDead)
            {
                SetCursor(CursorType.None);
                return;
            }

            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;
            SetCursor(CursorType.None);
        }

        private bool InteractWithComponent()
        {
            var hits = RaycastAllSorted();

            foreach (var hit in hits)
            {
                var raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (var raycastable in raycastables)
                {
                    if (raycastable.HandleRaycast(this))
                    {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }

            return false;
        }

        private RaycastHit[] RaycastAllSorted()
        {
            var hits = Physics.RaycastAll(GetMouseRay());
            BubbleSortByDistance(hits);
            return hits;
        }

        private static void BubbleSortByDistance(RaycastHit[] hits)
        {
            var distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);
        }

        private bool InteractWithUI()
        {
            // event system works through UI so GameObject refers to this
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            bool hasHit = RaycastNavMesh(out Vector3 target);

            if (hasHit)
            {
                if (!_mover.CanMoveTo(target)) return false;

                if (Input.GetMouseButton(0))
                {
                    _mover.StartMoveAction(target, 1f);
                }
                SetCursor(CursorType.Movement);
                return true;
            }
            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();

            bool hasHit = Physics.Raycast(GetMouseRay(), out RaycastHit hit);
            if (!hasHit) return false;

            bool hasCastToNavMesh = NavMesh.SamplePosition(
                hit.point, out NavMeshHit navMeshHit, _maxNavMeshProjection, NavMesh.AllAreas);

            if (!hasCastToNavMesh) return false;
                        
            target = navMeshHit.position;

            if (!_mover.CanMoveTo(target)) return false;

            return true;
        }

        private void SetCursor(CursorType cursorType)
        {
            CursorMapping mapping = GetCursorMapping(cursorType);
            Cursor.SetCursor(mapping.texture, mapping.hotSpot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (var mapping in _cursorMappings)
            {
                if (mapping.type == type) return mapping;   
            }

            return _cursorMappings[0];
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }

    [System.Serializable]
    internal struct CursorMapping
    {
        public CursorType type;
        public Texture2D texture;
        public Vector2 hotSpot;
    }
}