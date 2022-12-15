using System;
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
        [SerializeField] private bool playerSpeking = false;
        [SerializeField] private string onEnterAction;
        [SerializeField] private string onExitAction;
        

        public string GetText() => text;
        public Rect GetRect() => rect;
        public List<string> GetListOfNextDialogueNodes() => nextDialogueNodes;
        public bool IsPlayerSpeaking() => playerSpeking;
        public string GetOnEnterAction() => onEnterAction;
        public string GetOnExitAction()=> onExitAction;



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
        public void SetSpeaking(bool isPlayerSpeaking) 
        {
            Undo.RecordObject(this, "Speaking side chenged");
            playerSpeking = isPlayerSpeaking;        
        }

        internal string GetConversantName()
        {
            throw new NotImplementedException();
        }
#endif
    }
}