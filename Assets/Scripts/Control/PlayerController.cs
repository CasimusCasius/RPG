using RPG.Combat;
using RPG.Atributes;
using RPG.Movment;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.AI;
using RPG.Inventories;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;

        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;
        [SerializeField] float maxDistanceToNavMeshPoint = 1f;
        [SerializeField] float castRadius=0.5f;

        bool isDraggingUI = false;

        private void Awake()
        {
            health = GetComponent<Health>();
        }
        private void Update()
        {
            CheckSpecialAbilityKeys();
            if (InteractWithUI()) return;
            if (health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }
            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }
        private void CheckSpecialAbilityKeys()
        {
            var actionStore = GetComponent<ActionStore>();
            if (Input.GetKeyDown(KeyCode.Alpha1)) actionStore.Use(0, gameObject);
            if (Input.GetKeyDown(KeyCode.Alpha2)) actionStore.Use(1, gameObject);
            if (Input.GetKeyDown(KeyCode.Alpha3)) actionStore.Use(2, gameObject);
            if (Input.GetKeyDown(KeyCode.Alpha4)) actionStore.Use(3, gameObject);
            if (Input.GetKeyDown(KeyCode.Alpha5)) actionStore.Use(4, gameObject);
            if (Input.GetKeyDown(KeyCode.Alpha6)) actionStore.Use(5, gameObject);
        }

        private bool InteractWithComponent()
        {
            foreach (RaycastHit hit in RaycastAllSorted())
            {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
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
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(),castRadius);
            float[] distances = new float[hits.Length];
            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }
            Array.Sort(distances, hits);

            return hits;
        }

        private bool InteractWithUI()
        {
            if (Input.GetMouseButtonUp(0)) isDraggingUI = false;
            if (EventSystem.current.IsPointerOverGameObject())
            {
                if(Input.GetMouseButtonDown(0)) isDraggingUI = true;
                SetCursor(CursorType.UI);
                return true;
            }
            if (isDraggingUI) return true;
            return false;
        }
        private bool InteractWithMovement()
        {

            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            if (hasHit)
            {
                if (!GetComponent<Mover>().CanMoveTo(target)) return false;

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(target, 1f);
                }
                SetCursor(CursorType.Movement);
            }
            else
            {
                SetCursor(CursorType.None);
            }

            return hasHit;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            RaycastHit raycastHit;
            target = new Vector3();
            if (!Physics.Raycast(GetMouseRay(), out raycastHit)) return false;

            NavMeshHit hit;
            bool hasHit = NavMesh.SamplePosition(raycastHit.point, out hit, maxDistanceToNavMeshPoint, NavMesh.AllAreas);
            if (!hasHit) return false;
            target = hit.position;
            return true;
        }

        private static Ray GetMouseRay() => Camera.main.ScreenPointToRay(Input.mousePosition);
        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping cursor in cursorMappings)
            {
                if (cursor.type == type) return cursor;
            }
            return cursorMappings[0];
        }
        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }
    }
}
