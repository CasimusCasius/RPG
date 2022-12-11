using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Game/Dialogue", order = 0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] List<DialogueNode> nodes = new List<DialogueNode>();

        Dictionary<string, DialogueNode> nodeLookup = new Dictionary<string, DialogueNode>();


        private void OnValidate()
        {
            nodeLookup.Clear();
            foreach (DialogueNode node in nodes)
            {
                nodeLookup[node.uniqueID] = node;
            }
        }

        private void Awake()
        {
#if UNITY_EDITOR
            if (nodes.Count == 0)
            {
                DialogueNode rootNode= new DialogueNode();
                rootNode.uniqueID = System.Guid.NewGuid().ToString();
                nodes.Add(rootNode);
            }
#endif
            OnValidate();
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
            if (parentNode.NextDialogueNodes == null) yield break;
            foreach (string nodeChild in parentNode.NextDialogueNodes)
            {
                if (!nodeLookup.ContainsKey(nodeChild)) continue;
                yield return nodeLookup[nodeChild];
            }
        }

        public void CreateNode(DialogueNode parent)
        {
            DialogueNode addedNode = new DialogueNode();
            addedNode.uniqueID = System.Guid.NewGuid().ToString();

            parent.NextDialogueNodes.Add(addedNode.uniqueID);
            nodes.Add(addedNode);
            OnValidate();
        }
    }
}
