using RPG.Combat;
using RPG.Atributes;
using RPG.Movment;
using UnityEngine;
using System;
using UnityEngine.EventSystems;
using UnityEngine.AI;

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
        [SerializeField] float maxPathLenght=40f;

        private void Awake()
        {
            health = GetComponent<Health>();
        }
        private void Update()
        {

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

        private bool InteractWithComponent()
        {
            ;

            foreach (RaycastHit hit in RaycastAllSorted())
            {
                IRaycastable[] raycastables =  hit.transform.GetComponents<IRaycastable>();
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
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            float[] distances = new float[hits.Length];
            for (int i =0 ; i < hits.Length; i++)  
            {
                distances[i] =  hits[i].distance;
            }
            Array.Sort(distances, hits);

            return hits;
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }
            return false;
        }
        private bool InteractWithMovement()
        {
            
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);
            if (hasHit)
            {
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
            bool hasHit = NavMesh.SamplePosition(raycastHit.point,out hit, maxDistanceToNavMeshPoint, NavMesh.AllAreas);
            if(!hasHit) return false;

            target = hit.position;

            NavMeshPath path = new NavMeshPath();
            bool hasPath = NavMesh.CalculatePath(transform.position, target, NavMesh.AllAreas, path);
            if(!hasPath) return false;
            if(path.status != NavMeshPathStatus.PathComplete) return false;
            if(GetPathLenght(path)>maxPathLenght) return false;

            return true;
        }
        private float GetPathLenght(NavMeshPath path)
        {
            Vector3[] corners = path.corners;
            float distance=0f;
            if (corners.Length < 2) return distance;
            for (int i = 1; i < corners.Length; i++)
            {
                distance += Vector3.Distance(corners[i-1], corners[i]);
            }

            return distance;
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
