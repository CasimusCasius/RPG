using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogue.Editor
{

    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;
        Vector2 scrollPosition = new Vector2();
        [NonSerialized] GUIStyle nodeStyle;
        [NonSerialized] DialogueNode draggingNode = null;
        [NonSerialized] Vector2 draggingOffset = new Vector2();
        [NonSerialized] DialogueNode creatingNode = null;
        [NonSerialized] DialogueNode deletingNode = null;
        [NonSerialized] DialogueNode linkingParentNode = null;
        [NonSerialized] Vector2 offsetMousePosition = new Vector2();

        const float CANVAS_SIZE = 4000;
        const float BACKGROUND_SIZE = 50;

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
            nodeStyle.border = new RectOffset(24, 24, 24, 24);
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

                scrollPosition =  GUILayout.BeginScrollView(scrollPosition);
                Rect canvas = GUILayoutUtility.GetRect(CANVAS_SIZE,CANVAS_SIZE);
                Texture2D texture = Resources.Load("background") as Texture2D;
                Rect textureCoords = new Rect(0, 0, CANVAS_SIZE / BACKGROUND_SIZE, CANVAS_SIZE / BACKGROUND_SIZE);
                GUI.DrawTextureWithTexCoords( canvas, texture, textureCoords);

                foreach (var node in selectedDialogue.GetAllNodes())
                {
                    DrawConection(node);
                }
                foreach (var node in selectedDialogue.GetAllNodes())
                {
                    DrawNode(node);
                }
                GUILayout.EndScrollView();
                if (creatingNode != null)
                {
                    selectedDialogue.CreateNode(creatingNode);   
                    creatingNode = null;
                }
                if (deletingNode != null)
                {
                    selectedDialogue.DeleteNode(deletingNode);
                    deletingNode = null;
                }
            }
        }
        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && draggingNode == null)
            {
                draggingNode = GetNodeAtPoint(Event.current.mousePosition+scrollPosition);
                if (draggingNode != null)
                {
                    draggingOffset = draggingNode.GetRect().position - Event.current.mousePosition;
                    Selection.activeObject = draggingNode;
                }
                else
                {
                    offsetMousePosition = Event.current.mousePosition;
                    Selection.activeObject = selectedDialogue;
                }
            }
            else if (Event.current.type == EventType.MouseDrag)
            {
                if (draggingNode != null)
                {
                    draggingNode.SetRectPosition(Event.current.mousePosition + draggingOffset);
                    GUI.changed = true;
                }
                else
                {
                    offsetMousePosition -= Event.current.mousePosition;
                   
                    scrollPosition += offsetMousePosition;
                    offsetMousePosition = Event.current.mousePosition;

                    GUI.changed = true;
                }
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
                if (node.GetRect().Contains(mousePosition))
                {
                    foundNode = node;
                }
            }
            return foundNode;
        }
        private void DrawNode(DialogueNode node)
        {
            GUILayout.BeginArea(node.GetRect(), nodeStyle);

            node.SetText(EditorGUILayout.TextField(node.GetText()));

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("+"))
            {
                creatingNode = node;
            }

            DrawLinkButtons(node);

            if (GUILayout.Button("-"))
            {
                deletingNode = node;
            }
            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void DrawLinkButtons(DialogueNode node)
        {
            if (linkingParentNode == null)
            {
                if (GUILayout.Button("link"))
                {
                    linkingParentNode = node;
                }
            }
            else if (linkingParentNode == node)
            {
                if (GUILayout.Button("cancel"))
                {

                    linkingParentNode = null;
                }
            }
            else if (linkingParentNode.GetListOfNextDialogueNodes().Contains(node.name))
            {
                if (GUILayout.Button("unlink"))
                {
                    
                    linkingParentNode.RemoveNextDialogueNode(node.name);
                    linkingParentNode = null;
                }
            }
            else
            {
                if (GUILayout.Button("child"))
                {
                    
                    linkingParentNode.AddNextDialogueNode(node.name);
                    linkingParentNode = null;
                }

            }
        }
        private void DrawConection(DialogueNode node)
        {
            float lineWidth = 4f;
            float curveFactor = 0.8f;
            float pointOffset = 5f;
            Vector3 startPoint = new Vector2(node.GetRect().xMax - pointOffset, node.GetRect().center.y);

            foreach (var childNode in selectedDialogue.GetAllChildren(node))
            {
                Vector3 endPoint = new Vector2(childNode.GetRect().xMin + pointOffset, childNode.GetRect().center.y);
                Vector3 controlPointOffset = endPoint - startPoint;
                controlPointOffset.y = 0;
                controlPointOffset.x *= curveFactor;
                Handles.DrawBezier(
                    startPoint,
                    endPoint,
                    startPoint + controlPointOffset,
                    endPoint - controlPointOffset,
                    Color.blue, null,
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