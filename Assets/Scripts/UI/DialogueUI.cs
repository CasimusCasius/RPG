using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RPG.Dialogue;
using TMPro;
using UnityEngine.UI;

namespace RPG.UI
{ 
    public class DialogueUI : MonoBehaviour
    {
        PlayerConversant playerConversant;
        [SerializeField] TextMeshProUGUI AIText;
        [SerializeField] Transform AIResponce;  
        [SerializeField] Button nextButton;
        [SerializeField] Transform choiseRoot;
        [SerializeField] Button choiseButtonPrefab;

        private void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            playerConversant = player.GetComponent<PlayerConversant>();
            nextButton.onClick.AddListener(Next);
            UpdateUI();
        }

        void Next()
        {
            playerConversant.Next();
            UpdateUI();
        }

        private void UpdateUI()
        {
            AIResponce.gameObject.SetActive(!playerConversant.IsChoosing());
            choiseRoot.gameObject.SetActive(playerConversant.IsChoosing());

            if (playerConversant.IsChoosing())
            {
                DestroyChoiseButtons();

                foreach (var choice in playerConversant.GetChoices())
                {
                    var choiseInst = Instantiate(choiseButtonPrefab, choiseRoot);
                    choiseInst.GetComponentInChildren<TextMeshProUGUI>().text = choice.GetText();
                }
            }
            else
            {
                AIText.text = playerConversant.GetText();
                nextButton.gameObject.SetActive(playerConversant.HasNext());
            }
        }

        private void DestroyChoiseButtons()
        {
            foreach (Transform item in choiseRoot)
            {
                Destroy(item.gameObject);
            }
        }
    }
}
