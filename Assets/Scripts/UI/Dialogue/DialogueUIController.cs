using System.Collections;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using static PlayerClueController;

public class DialogueUIController : MonoBehaviour, IPointerClickHandler
{

    [Header("Portrait")]
    DialogueCharacterSO Character;
    public Image Head;
    public Image Face;

    [Header("Components")]

    public int wordsPerSecond = 10;
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI nameText;
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
        if (talkingNPC == null)
        {
            UIController.main.OpenWindow(UIController.UIWindow.dialogue);
        }
        if (CutsceneCoroutine!=null)
            StopCoroutine(CutsceneCoroutine);

        CutsceneCoroutine = StartCoroutine(PlayCutsceneCoroutine(Script));
    }
    public bool IsCutscenePlaying()
    {
        return gameObject.activeSelf && CutsceneCoroutine != null;
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
            if (talkingNPC == null)
            {
                Close();
            }
            else
            {
                yield return LoadNPCQuery();
            }
        }
    }
    public void Close()
    {
        if (CutsceneCoroutine!=null)
        StopCoroutine(CutsceneCoroutine);
        CutsceneCoroutine = null;

        UIController.main.CloseWindow();
    }
    public IEnumerator LoadDialogueLine(DialogueScriptSO.DialogueLine NewLine)
    {
        if (NewLine.Character == null && talkingNPC != null)
        {
            ChangeCharacter(talkingNPC.CharacterFile);
        }
        else
        {
            ChangeCharacter(NewLine.Character);
        }
        EmoteCharacter(NewLine.Emotion);

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
        float Wait = GetWaitValue(NewLine.Quote);
        if (Wait > 0)
        {
            SkipLine = false;
            yield return SkippableWait(Wait);
        }
    }
    bool SkipLine = false;
    public IEnumerator TypeDialog(string dialog)
    {
        SkipLine = false;
        dialogText.text = "";

        char[] line = dialog.ToCharArray();

        for (int iC = 0; iC < line.Length; iC++)
        {
            if (line[iC] != ' ')
            {
                bool writeWord = true;
                while (iC < line.Length && writeWord)
                {
                    dialogText.text += line[iC];
                    iC++;
                    if (iC < line.Length && line[iC] == ' ')
                    {
                        dialogText.text += line[iC];
                        writeWord = false;
                    }
                }
            }

            if (SkipLine)
                break;
            yield return SkippableWait(1f / wordsPerSecond);
        }
        dialogText.text = dialog;
    }
    float GetWaitValue(string line)
    {
        return 2f + line.Length * .03f; 
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

    public void ChangeCharacter(DialogueCharacterSO character)
    {
        if (Character == character)
            return;
        Character = character;
        Head.gameObject.SetActive(Character != null);
        Face.gameObject.SetActive(Character != null);
        if (Character != null)
        {
            Head.sprite = Character.Head;
            EmoteCharacter(DialogueScriptSO.CharacterEmotion.none);
            if (Character.Faces.Length > 0)
                Face.sprite = Character.Faces[0];
            else
                Face.sprite = null;
        }
    }
    public void EmoteCharacter(DialogueScriptSO.CharacterEmotion emotion)
    {
        if (Character != null)
        {
            if (emotion >= 0 && (int)emotion < Character.Faces.Length)
            {
                Face.sprite = Character.Faces[(int)emotion];
                Face.enabled = true;
            }
            else
            {

                Face.enabled = false;
            }
                
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
    #region NPC Dialogue
    public CharacterSO talkingNPC;
    public void TalkWithNPC(CharacterSO talker)
    {
        HideMultipleChoice();
        talkingNPC = talker;

        UIController.main.OpenWindow(UIController.UIWindow.dialogue);
        if (CutsceneCoroutine != null)
            StopCoroutine(CutsceneCoroutine);

        CutsceneCoroutine = StartCoroutine(HandleNPCTalk());
    }
    public void ForgetNPC()
    {
        talkingNPC = null;
    }
    IEnumerator HandleNPCTalk()
    {
        if (talkingNPC.WelcomeLines.Length > 0)
        {
            yield return LoadNPCWelcome();
        }
        /*Close();
        UIController.main.IdeaManagerWindow.PuzzleBar.LoadChoices((ClueChoiceSO)Script.EndChoice);
        UIController.main.IdeaManagerWindow.gameObject.SetActive(true);
   */
        UIController.main.IdeaManagerWindow.gameObject.SetActive(true);

        if (talkingNPC.Questions.Length > 0)
        {
            yield return LoadNPCQuery();
        }
    }
    IEnumerator LoadNPCWelcome()
    {
        int randomLine = Random.Range(0, talkingNPC.WelcomeLines.Length - 1);
        string welcome = talkingNPC.WelcomeLines[randomLine];
        yield return ShowNPCDialogue(welcome);
    }
    IEnumerator LoadNPCQuery()
    {
        int randomLine = Random.Range(0, talkingNPC.Questions.Length - 1);
        string question = talkingNPC.Questions[randomLine];
        yield return ShowNPCDialogue(question);
    }
    public IEnumerator ShowNPCDialogue(string Dialogue)
    {
        if (talkingNPC != null)
        {
            ChangeCharacter(talkingNPC.CharacterFile);
            EmoteCharacter(DialogueScriptSO.CharacterEmotion.normal);
            EmoteCharacter(DialogueScriptSO.CharacterEmotion.normal);

            if (talkingNPC.WelcomeLines.Length > 0)
            {
                yield return TypeDialog(Dialogue);
                float Wait = GetWaitValue(Dialogue);
                if (Wait > 0)
                {
                    SkipLine = false;
                    yield return SkippableWait(Wait);
                }
            }
        }
    }
    #endregion
    public void OnPointerClick(PointerEventData eventData)
    {
        int linkIndex = TMP_TextUtilities.FindIntersectingLink(dialogText, Input.mousePosition, null);
        if (linkIndex >= 0 && linkIndex < dialogText.textInfo.linkInfo.Length)
        HandleLinkFunctions( dialogText.textInfo.linkInfo[linkIndex].GetLinkID());
    }
    bool HandleLinkFunctions(string linkID)
    {
        if (linkID.Substring(0,4) == "var_")
        {
            Debug.Log(linkID);
            Debug.Log(linkID.Substring(4, linkID.Length - 4));
            PlayerClueController.main.RevealClue(linkID.Substring(4,linkID.Length-4).ToLower(), true);
            return true;
        }
        return false;
    }
}
