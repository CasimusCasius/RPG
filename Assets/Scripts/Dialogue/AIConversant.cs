using RPG.Control;
using System;
using Unity.Mathematics;
using UnityEngine;

namespace RPG.Dialogue
{
    public class AIConversant : MonoBehaviour, IRaycastable

    {
        [SerializeField] string conversantName;
        [SerializeField] Dialogue dialogue = null;

        public CursorType GetCursorType()
        {
            return CursorType.Dialogue;
        }

        public bool HandleRaycast(PlayerController callingControler)
        {
            if (!enabled) { return false; }
            if (dialogue == null) return false;

            if (Input.GetMouseButtonDown(0))
            {
                callingControler.GetComponent<PlayerConversant>().StartDialogue(this,dialogue);
            }
            return true;
        }

        public string GetConversantName() => conversantName;
    }
}
