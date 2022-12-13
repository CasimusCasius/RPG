using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] Dialogue currentDialogue;
        DialogueNode currentNode;
        bool isChoosing = false;

        private void Awake()
        {
            currentNode = currentDialogue.GetRootNode();
        }
        public bool IsChoosing()
        {
            return isChoosing;
        }
        public string GetText()
        {
            if (currentDialogue == null) return "";
            return currentNode.GetText();
        }  

        public void Next()
        {
            int numOfPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();
            if (numOfPlayerResponses > 0)
            {
                isChoosing= true;
                return;
            }
            var childs = currentDialogue.GetAIChildren(currentNode).ToArray();
            int i = Random.Range(0, childs.Length);
            currentNode = childs[i];
        }

        public IEnumerable<DialogueNode> GetChoices()
        {
            return currentDialogue.GetPlayerChildren(currentNode);
        }
        public bool HasNext()
        {
            
            return currentNode.GetListOfNextDialogueNodes().Count > 0;
        }
    }
}

