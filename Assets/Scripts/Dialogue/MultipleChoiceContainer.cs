using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MultipleChoiceContainer : MonoBehaviour
{
    public TextMeshProUGUI Header;
    public Button[] ChoiceButtons;
    public TextMeshProUGUI[] ChoiceText;

    private void Awake()
    {
        if (ChoiceButtons == null || ChoiceButtons.Length == 0)
        {
            ChoiceButtons = GetComponentsInChildren<Button>();

            List<TextMeshProUGUI> texts = new List<TextMeshProUGUI>();
            int iB = 0;
            foreach (Button btn in ChoiceButtons)
            {
                texts.Add(btn.GetComponentInParent<TextMeshProUGUI>());
                int iF = iB;
                btn.onClick.AddListener(() =>
                {
                    OnChoiceSelect(iF);
                });
                iB++;
            }
            ChoiceText = texts.ToArray();
        }
        else
        {
            int iB = 0;
            foreach (Button btn in ChoiceButtons)
            {
                int iF = iB;
                btn.onClick.AddListener(() =>
                {
                    OnChoiceSelect(iF);
                });
                iB++;
            }
        }
    }
    void OnChoiceSelect(int choice)
    {
        if (myChoices == null || choice >= myChoices.Choices.Length)
            return;

        MultipleChoiceSO.Choice action = myChoices.Choices[choice];
        if (action == null)
            return;

        if (action.dialogue!=null)
        {
            UIController.main.dialogueController.PlayCutscene(action.dialogue);
        }
        else
        {
            UIController.main.dialogueController.Close();
        }
    }
    MultipleChoiceSO myChoices;
    public void LoadMultipleChoiceFunction(MultipleChoiceSO MultipleChoiceScriptable)
    {
        myChoices = MultipleChoiceScriptable;
        for (int iC = 0; iC< ChoiceButtons.Length; iC++)
        {
            if (iC < MultipleChoiceScriptable.Choices.Length)
            {
                ChoiceButtons[iC].gameObject.SetActive(true);
                ChoiceButtons[iC].interactable = true;
                ChoiceText[iC].text = MultipleChoiceScriptable.Choices[iC].text;
            }
            else
            {
                ChoiceButtons[iC].gameObject.SetActive(false);
            }
        }
    }
}
