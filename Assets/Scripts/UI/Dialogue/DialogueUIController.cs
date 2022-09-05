using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUIController : MonoBehaviour
{

    [Header("Portrait 1")]
    DialogueCharacterSO Char1;
    public Image Head1;
    public Image Eyes1;
    public Image Mouth1;

    [Header("Components")]

    public int letterPerSecond = 60;
    public Text dialogText;
    public Text nameText;
    public MultipleChoiceContainer MultipleChoice;

    Coroutine CutsceneCoroutine;
    private void Update()
    {
        HandleExit();
    }
    void HandleExit()
    {
        if (CutsceneCoroutine!=null) //TODO define skip key
        {
            if (Input.GetKeyDown(KeyCode.Z))
            {
                SkipLine = true;
            }
            /*if (Input.GetKey(KeyCode.X))
            {
                skipPercent += Time.deltaTime;
                skipText.color = new Color(1, 1, 1, skipPercent);
            }
            else
            {
                skipPercent = 0;
                skipText.color = Color.clear;
            }
            if (skipPercent > 1)
            {
                StopCoroutine(CutsceneCoroutine);
                CutsceneCoroutine = null;
            }*/
        }
    }
    public void PlayCutscene(DialogueScriptSO Script)
    {
        UIController.main.OpenWindow(UIController.UIWindow.dialogue);
        if (CutsceneCoroutine!=null)
        StopCoroutine(CutsceneCoroutine);

        CutsceneCoroutine = StartCoroutine(PlayCutsceneCoroutine(Script));
    }
    public bool IsCutscenePlaying()
    {
        return CutsceneCoroutine != null;
    }
    IEnumerator PlayCutsceneCoroutine(DialogueScriptSO Script)
    {
        HideMultipleChoice();
        int quote = 0;
        while (quote< Script.Dialogue.Length)
        {
            yield return LoadDialogueLine(Script.Dialogue[quote]);
            quote++;
        }
        if (Script.EndChoice != null)
        {
            if (Script.EndChoice.GetType() == typeof(MultipleChoiceSO))
            {
                ShowMultipleChoice((MultipleChoiceSO)Script.EndChoice);
            }
            if (Script.EndChoice.GetType() == typeof(ClueChoiceSO))
            {
                Close();
                UIController.main.IdeaManagerWindow.PuzzleBar.LoadChoices((ClueChoiceSO)Script.EndChoice);
                UIController.main.IdeaManagerWindow.gameObject.SetActive(true);
            }

        waitloop:
            yield return new WaitForEndOfFrame();
            goto waitloop;
        }
        else
        {
            Close();
        }
    }
    public void Close()
    {
        StopCoroutine(CutsceneCoroutine);
        CutsceneCoroutine = null;

        UIController.main.CloseWindow();
    }
    public IEnumerator LoadDialogueLine(DialogueScriptSO.DialogueLine NewLine)
    {
        ChangeCharacter(NewLine.Character);
        EmoteCharacter(NewLine.Eyes);
        EmoteCharacter(NewLine.Mouth);

        if (NewLine.PlayType == DialogueScriptSO.AudioPlayType.before)
        {
            if (NewLine.AudioClip != null)
                PlaySound(NewLine.AudioClip);
            if (NewLine.DialogueShake > 0)
                StartCoroutine(DialogueShake(NewLine.DialogueShake, 10));
        }
        if (NewLine.Quote.Length > 0)
        {
            yield return TypeDialog(NewLine.Quote);
            if (NewLine.PlayType == DialogueScriptSO.AudioPlayType.after)
            {
                if (NewLine.AudioClip != null)
                    PlaySound(NewLine.AudioClip);
                if (NewLine.DialogueShake > 0)
                    StartCoroutine(DialogueShake(NewLine.DialogueShake,10));
            }
        }
        if (NewLine.Wait > 0)
        {
            SkipLine = false;
            yield return SkippableWait(NewLine.Wait);
        }
    }
    bool SkipLine = false;
    public IEnumerator TypeDialog(string dialog)
    {
        SkipLine = false;
        dialogText.text = "";
        foreach (var letter in dialog.ToCharArray())
        {
            dialogText.text += letter;
            if (SkipLine)
                break;
            yield return SkippableWait(1f / letterPerSecond);
        }
        dialogText.text = dialog;
    }
    IEnumerator SkippableWait(float Dur)
    {
        float Wait = Time.time + Dur;
        while (Wait > Time.time)
        {
            if (SkipLine)
                break;
            yield return new WaitForEndOfFrame();
        }
    }

    public void ChangeCharacter( DialogueCharacterSO character)
    {
                if (Char1 == character)
                    return;
                Char1 = character;
                Head1.gameObject.SetActive(Char1 != null);
                Eyes1.gameObject.SetActive(Char1 != null);
                Mouth1.gameObject.SetActive(Char1 != null);
                if (Char1 != null)
                {
                    Head1.sprite = Char1.Base;
                    Eyes1.sprite = Char1.Eyes[0];
                    Mouth1.sprite = Char1.Mouths[0];
                }
    }
    public void EmoteCharacter(DialogueScriptSO.CharacterMouthPosition emotion)
    {
                if (Char1 != null)
                {
                    Mouth1.sprite = Char1.Mouths[(int)emotion];
                }
    }
    public void EmoteCharacter( DialogueScriptSO.CharacterEyePosition emotion)
    {
                if (Char1 != null)
                {
                    Eyes1.sprite = Char1.Eyes[(int)emotion];
                }
    }

    public void PlaySound(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        //audiosrc.pitch = pitch;
        //audiosrc.PlayOneShot(clip, volume);
    }

    public IEnumerator CameraShake(float duration, float magnitude)
    {
        Vector3 originalPos = Camera.main.transform.localPosition;
        float timer = 0.0f;

        while (timer < duration)
        {
            float x = (Random.Range(-1f, 1f) * magnitude);
            float y = (Random.Range(-1f, 1f) * magnitude);

            Camera.main.transform.localPosition = new Vector3(x, y, originalPos.z);

            timer +=  Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        Camera.main.transform.localPosition = originalPos;
    }

    public IEnumerator DialogueShake(float duration, float magnitude)
    {
        RectTransform tRect = GetComponent<RectTransform>();
        Vector3 originalPos = tRect.anchoredPosition;
        float timer = 0.0f;

        while (timer < duration)
        {
            float x = (Random.Range(-1f, 1f) * magnitude);
            float y = (Random.Range(-1f, 1f) * magnitude);

            tRect.anchoredPosition = new Vector3(x, y, originalPos.z);

            timer +=  Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        tRect.anchoredPosition = originalPos;
    }

    public void ShowMultipleChoice(MultipleChoiceSO choices)
    {
        MultipleChoice.gameObject.SetActive(true);
        MultipleChoice.LoadMultipleChoiceFunction(choices);
    }
    public void HideMultipleChoice()
    {
        MultipleChoice.gameObject.SetActive(false);
    }
}
