using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.MPE;
using UnityEngine;

namespace RPG.Dialogue.Editor
{

    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;
        [NonSerialized] GUIStyle nodeStyle;
        [NonSerialized] DialogueNode draggingNode = null;
        [NonSerialized] Vector2 draggingOffset = new Vector2();
        [NonSerialized] DialogueNode creatingNode = null;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow()
        {
            GetWindow<DialogueEditor>(false, "DialogueEditor");
        }
        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceId, int line)
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

            nodeStyle = new GUIStyle();
            nodeStyle.normal.background = (Texture2D)EditorGUIUtility.Load("node3");
            nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            nodeStyle.border = new RectOffset(24,24,24,24);
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
                ProcessEvents();
                foreach (var node in selectedDialogue.GetAllNodes())
                {
                    DrawConection(node);
                }
                foreach (var node in selectedDialogue.GetAllNodes())
                {
                    DrawNode(node);
                }
                if (creatingNode != null)
                {
                    Undo.RecordObject(selectedDialogue, "Add node");
                    selectedDialogue.CreateNode(creatingNode);
                    creatingNode = null;
                }
            }
        }
        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && draggingNode == null)
            {

                draggingNode = GetNodeAtPoint(Event.current.mousePosition);
                if (draggingNode != null)
                    draggingOffset = draggingNode.rect.position - Event.current.mousePosition;
            }
            else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {
                Undo.RecordObject(selectedDialogue, "Update Node Position");
                draggingNode.rect.position = Event.current.mousePosition + draggingOffset;
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && draggingNode != null)
            {
                draggingNode = null;
            }
        }
        private DialogueNode GetNodeAtPoint(Vector2 mousePosition)
        {
            DialogueNode foundNode = null;
            foreach (var node in selectedDialogue.GetAllNodes())
            {
                if (node.rect.Contains(mousePosition))
                {
                    foundNode = node;
                }
            }
            return foundNode;
        }
        private void DrawNode(DialogueNode node)
        {

            GUILayout.BeginArea(node.rect, nodeStyle);
            EditorGUI.BeginChangeCheck();
            
            string testString = EditorGUILayout.TextField(node.text);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(selectedDialogue, "Update Dialogue Text");
                node.text = testString;
            }
            if (GUILayout.Button("+"))
            {
                creatingNode = node;
            }
            GUILayout.EndArea();
        }
        private void DrawConection(DialogueNode node)
        {
            float lineWidth = 4f;
            float curveFactor = 0.8f;
            float pointOffset = 5f;
            Vector3 startPoint = new Vector2(node.rect.xMax - pointOffset, node.rect.center.y);
            
            foreach (var childNode in selectedDialogue.GetAllChildren(node))
            {
                Vector3 endPoint = new Vector2(childNode.rect.xMin + pointOffset, childNode.rect.center.y);
                Vector3 controlPointOffset = endPoint- startPoint;
                controlPointOffset.y = 0;
                controlPointOffset.x *= curveFactor;
                Handles.DrawBezier(
                    startPoint,
                    endPoint,
                    startPoint + controlPointOffset,
                    endPoint-controlPointOffset, 
                    Color.blue,null, 
                    lineWidth);
            }
        }
        private void onSelectionChanged()
        {
            if (Selection.activeObject as Dialogue != null)
            {
                selectedDialogue = (Dialogue)Selection.activeObject;
                Repaint();
            }
        }
    }
}