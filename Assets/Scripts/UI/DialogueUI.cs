using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Dialogue;
using TMPro;

namespace RPG.UI
{ 
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant playerConversant;
        [SerializeField] TextMeshProUGUI AIText;

        private void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            playerConversant = player.GetComponent<PlayerConversant>();
            SetAIText();
        }

        private void SetAIText()
        {
            Debug.Log(playerConversant.GetText());
            AIText.text = playerConversant.GetText();
        }

    }
}
