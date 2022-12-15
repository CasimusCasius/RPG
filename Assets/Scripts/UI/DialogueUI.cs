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
        [SerializeField] Button quitButton;
        [SerializeField] TextMeshProUGUI conversantName;

        private void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            playerConversant = player.GetComponent<PlayerConversant>();
            playerConversant.onConversationUpdated += UpdateUI;
            nextButton.onClick.AddListener(() => playerConversant.Next());
            quitButton.onClick.AddListener(()=>playerConversant.Quit());

            UpdateUI();
        }
        private void UpdateUI()
        {
            gameObject.SetActive(playerConversant.IsActive());
            if (!playerConversant.IsActive()) return;
            conversantName.text = playerConversant.GetCurrentConversantName();
            AIResponce.gameObject.SetActive(!playerConversant.IsChoosing());
            choiseRoot.gameObject.SetActive(playerConversant.IsChoosing());

            if (playerConversant.IsChoosing())
            {
                BuildChoiceList();
            }
            else
            {
                AIText.text = playerConversant.GetText();
                nextButton.gameObject.SetActive(playerConversant.HasNext());
            }
        }
        private void BuildChoiceList()
        {
            DestroyChoiseButtons();

            foreach (var choice in playerConversant.GetChoices())
            {
                var choiceInst = Instantiate(choiseButtonPrefab, choiseRoot);
                choiceInst.GetComponentInChildren<TextMeshProUGUI>().text = choice.GetText();

                Button button = choiceInst.GetComponentInChildren<Button>();
                button.onClick.AddListener(() =>
                {
                    playerConversant.SelectChoice(choice);
                }
                );
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
