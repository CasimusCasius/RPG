using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{

    public class DialogueNode : ScriptableObject
    {
        [SerializeField] private string text;
        [SerializeField] private List<string> nextDialogueNodes = new List<string>();
        [SerializeField] private Rect rect = new Rect(0, 0, 200, 100);


        public string GetText() => text;
        public Rect GetRect() => rect;
        public List<string> GetListOfNextDialogueNodes() => nextDialogueNodes;


#if UNITY_EDITOR
        public void AddNextDialogueNode(string childID)
        {
            Undo.RecordObject(this, "Update Next Dialogue node ");
            nextDialogueNodes.Add(childID);
            EditorUtility.SetDirty(this);
        }
        public void RemoveNextDialogueNode(string childID)
        {
            Undo.RecordObject(this, "Remove Next Dialogue node ");
            nextDialogueNodes.Remove(childID);
            EditorUtility.SetDirty(this);
        }

        public void SetText(string newText)
        {
            if (text != newText)
            {
                Undo.RecordObject(this, "Update Dialogue Text");
                text = newText;
                EditorUtility.SetDirty(this);
            }
        }

        public void SetRectPosition(Vector2 position)
        {
            Undo.RecordObject(this, "Update Rect Position");
            rect.position = position;
            EditorUtility.SetDirty(this);
        }
#endif
    }
}