using RPG.Combat;
using RPG.Atributes;
using RPG.Movment;
using UnityEngine;
using System;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;
        enum CursorType
        {
            None,
            Movement,
            Combat
        }
        [System.Serializable]
        struct CursorMapping
        {
            public CursorType type;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] CursorMapping[] cursorMappings = null;


        private void Awake()
        {
            health = GetComponent<Health>();
        }
        private void Update()
        {
            if (health.IsDead())
            {
                return;
            }
            if (InteractWithCombat()) return;
            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }
        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());

            foreach (RaycastHit hit in hits)
            {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;

                if (!GetComponent<Fighter>().CanAttack(target.gameObject)) continue;

                if (Input.GetMouseButton(0))
                {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }
                SetCursor(CursorType.Combat);
                return true;
            }
            return false;
        }
        private static Ray GetMouseRay() => Camera.main.ScreenPointToRay(Input.mousePosition);

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                if (Input.GetMouseButton(0))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f);
                }
                SetCursor(CursorType.Movement);
            }
            return hasHit;
        }
        private CursorMapping GetCursorMapping(CursorType type)
        {
            foreach (CursorMapping cursor in cursorMappings)
            {
                if (cursor.type==type) return cursor;
            }
            return cursorMappings[0];
        }
        private void SetCursor(CursorType type)
        {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture,mapping.hotspot,CursorMode.Auto);
        }
    }
}
