using RPG.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogue
{
    public class PlayerConversant : MonoBehaviour
    {
        [SerializeField] string playerName;
       
        Dialogue currentDialogue;
        DialogueNode currentNode;
        bool isChoosing = false;
        AIConversant currentConversant= null;

        public event Action onConversationUpdated;



        public void StartDialogue(AIConversant newConversant, Dialogue dialogue)
        {
            currentDialogue = dialogue;
            currentNode = currentDialogue.GetRootNode();
            currentConversant = newConversant;
            TriggerEnterAction();
            onConversationUpdated?.Invoke();
        }
        public void Quit()
        {
            TriggerExitAction();
            currentConversant = null;
            currentDialogue = null;
            currentNode = null;
            isChoosing = false;

            onConversationUpdated?.Invoke();
        }

        public bool IsActive()
        { return currentDialogue != null; }

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
            TriggerExitAction();
            int numOfPlayerResponses = currentDialogue.GetPlayerChildren(currentNode).Count();
            if (numOfPlayerResponses > 0)
            {
                isChoosing= true;
                onConversationUpdated?.Invoke();

                return;
            }
            var childs = currentDialogue.GetAIChildren(currentNode).ToArray();
            int i = UnityEngine.Random.Range(0, childs.Length);
            
            currentNode = childs[i];
            TriggerEnterAction();
            onConversationUpdated?.Invoke();
        }
        public IEnumerable<DialogueNode> GetChoices()
        {
            return currentDialogue.GetPlayerChildren(currentNode);
        }
        public void SelectChoice(DialogueNode chosenNode)
        {
            if (chosenNode == null) return;
            
            currentNode = chosenNode;
            isChoosing = false;
            TriggerEnterAction();
            Next();
        }
        public bool HasNext() =>currentNode.GetListOfNextDialogueNodes().Count > 0;

        public string GetCurrentConversantName()
        {
            if (isChoosing) return playerName;

            return currentConversant.GetConversantName();
        }

        private void TriggerEnterAction()
        {
            if(currentNode != null )
            {
               TriggerAction(currentNode.GetOnEnterAction());
            }

        }
        private void TriggerExitAction()
        {
             if(currentNode != null )
            {
                TriggerAction(currentNode.GetOnExitAction());
            }
        }

        private void TriggerAction(string action)
        {
            var triggers = currentConversant.GetComponents<DialogueTrigger>();
            if (triggers == null || action == "") return;
            foreach (var trigger in triggers)
            {
                trigger.Trigger(action);
            }
        }


    }
}

