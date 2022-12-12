using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Game/Dialogue", order = 0)]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();
        [SerializeField] Vector2 newNodeOffset = new Vector2(200, 0);
        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();


        private void OnValidate()
        {
            nodeLookup.Clear();
           
            foreach (DialogueNode node in nodes)
            {
                nodeLookup[node.name] = node;
            }
        }
        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }
        public DialogueNode GetRootNode()
        {
            return nodes[0];
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode parentNode)
        {
            if (parentNode.GetListOfNextDialogueNodes() == null) yield break;
            foreach (string nodeChild in parentNode.GetListOfNextDialogueNodes())
            {
                if (!nodeLookup.ContainsKey(nodeChild)) continue;
                yield return nodeLookup[nodeChild];
            }
        }
#if UNITY_EDITOR
        public void CreateNode(DialogueNode parent)
        {
            DialogueNode addedNode = MakeNewNode(parent);
            Undo.RegisterCreatedObjectUndo(addedNode, "Create node");
            if (AssetDatabase.GetAssetPath(this) != "")
            {
                Undo.RecordObject(this, "Add node");
            }
            AddNode(addedNode);
            
        }
        private void AddNode(DialogueNode addedNode)
        {
            nodes.Add(addedNode);
            OnValidate();
        }

        private DialogueNode MakeNewNode(DialogueNode parent)
        {
            DialogueNode addedNode = CreateInstance<DialogueNode>();
            addedNode.name = System.Guid.NewGuid().ToString();

            if (parent != null)
            {
                parent.AddNextDialogueNode(addedNode.name);
                addedNode.SetRectPosition(parent.GetRect().position + newNodeOffset);
                addedNode.SetSpeaking(!parent.IsPlayerSpeaking());
            }

            return addedNode;
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            Undo.RecordObject(this, "Remove node");
            nodes.Remove(nodeToDelete);
            OnValidate();
            ClearDanglingChildren(nodeToDelete);
            Undo.DestroyObjectImmediate(nodeToDelete);
        }

        private void ClearDanglingChildren(DialogueNode nodeToDelete)
        {
            foreach (var node in GetAllNodes())
            {
                node.RemoveNextDialogueNode(nodeToDelete.name);
            }
        }
#endif
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (nodes.Count == 0)
            {
                DialogueNode addedNode = MakeNewNode(null);
                AddNode(addedNode);
            }

            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (var node in GetAllNodes())
                {
                    if (AssetDatabase.GetAssetPath(node) == "")
                    {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
                OnValidate();
            }
#endif
        }

        public void OnAfterDeserialize()
        {
            // Not interested
        }

    }
}
