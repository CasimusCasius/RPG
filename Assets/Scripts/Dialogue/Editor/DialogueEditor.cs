using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogue.Editor
{ 

    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow<DialogueEditor>(false,"DialogueEditor");
        }
        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceId,int line)
        {
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceId) as Dialogue;
            if (dialogue != null)
            {
                ShowEditorWindow();
                return true;
            }
            return false;
        }
        private void OnEnable()
        {
            Selection.selectionChanged += onSelectionChanged;
        }
        private void OnDisable()
        {
            Selection.selectionChanged += onSelectionChanged;
        }

        private void OnGUI()
        {
            if (selectedDialogue == null)
            {
                EditorGUILayout.LabelField("No dialogue selected.");
            }
            else
            {
                foreach (var node in selectedDialogue.GetAllNodes())
                {
                    EditorGUI.BeginChangeCheck();
                    EditorGUILayout.LabelField("Node:");
                    string id = EditorGUILayout.TextField(node.uniqueID);
                    string testString = EditorGUILayout.TextField(node.text);
                    
                    if (EditorGUI.EndChangeCheck())
                    {
                        Undo.RecordObject(selectedDialogue, "Update Dialogue Text");
                        node.uniqueID = id;
                        node.text = testString;
                    }
                }
            }
        }
        private void onSelectionChanged()
        {
            if (Selection.activeObject as Dialogue != null)
            {
                selectedDialogue = (Dialogue) Selection.activeObject;
                Repaint();
            }           
        }

    }
}