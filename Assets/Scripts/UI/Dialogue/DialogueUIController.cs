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
    public bool IsInDialogueMode()
    {
        return gameObject.activeSelf && CutsceneCoroutine != null || MultipleChoice.gameObject.activeSelf;
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
        if (talkingNPC == null)
        {
            Close();
        }
        else
        {
            yield return ShowNPCDialogue(talkingNPC.Question);
        }
    }
    public void Close()
    {
        if (CutsceneCoroutine!=null)
        StopCoroutine(CutsceneCoroutine);
        CutsceneCoroutine = null;

        UIController.main.CloseWindow();
    }
    public IEnumerator LoadDialogueLine(DialogueLineSO NewLine)
    {
        LoadDialogueCharacter(NewLine);

        if (NewLine.GetType() == typeof(DialogueAudioSO))
        {
            DialogueAudioSO dialogueAudioSO = (DialogueAudioSO)NewLine;
            if (dialogueAudioSO.PlayType == DialogueAudioSO.AudioPlayType.before)
            {
                if (dialogueAudioSO.AudioClip != null)
                    PlaySound(dialogueAudioSO.AudioClip);
            }
            string line = dialogueAudioSO.GetDialogueLine();
            if (line.Length > 0)
            {
                yield return TypeDialog(line);
                if (dialogueAudioSO.PlayType == DialogueAudioSO.AudioPlayType.after)
                {
                    if (dialogueAudioSO.AudioClip != null)
                        PlaySound(dialogueAudioSO.AudioClip);
                }
                yield return PostLineWait(line);
            }
        }
        else if (NewLine.GetType() == typeof(MultipleChoiceSO))
        {
            ShowMultipleChoice((MultipleChoiceSO)NewLine);
        }
        else if (NewLine.GetType() == typeof(ClueChoiceSO))
        {
            Close();
            UIController.main.IdeaManagerWindow.PuzzleBar.LoadChoices((ClueChoiceSO)NewLine);
            UIController.main.IdeaManagerWindow.gameObject.SetActive(true);
        }
        else
        {
            string DialogueQuestion = NewLine.GetDialogueLine();
            yield return TypeDialog(DialogueQuestion);
            yield return PostLineWait(DialogueQuestion);
        }
    }
    void LoadDialogueCharacter(DialogueLineSO NewLine)
    {
        if (NewLine.Character == null && talkingNPC != null)
        {
            ChangeCharacter(talkingNPC.CharacterFile);
        }
        else
        {
            ChangeCharacter(NewLine.Character);
        }
        EmoteCharacter(NewLine.Emote);
    }


    bool SkipLine = false;
    IEnumerator PostLineWait(string line)
    {
        float Wait = GetWaitValue(line);
        if (Wait > 0)
        {
            SkipLine = false;
            yield return SkippableWait(Wait);
        }
    }
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
            EmoteCharacter(DialogueLineSO.CharacterEmotion.none);
            if (Character.Faces.Length > 0)
                Face.sprite = Character.Faces[0];
            else
                Face.sprite = null;
        }
    }
    public void EmoteCharacter(DialogueLineSO.CharacterEmotion emotion)
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
        if (talkingNPC.WelcomeLine != null)
        {
            yield return ShowNPCDialogue(talkingNPC.WelcomeLine); 
        }
        /*Close();
        UIController.main.IdeaManagerWindow.PuzzleBar.LoadChoices((ClueChoiceSO)Script.EndChoice);
        UIController.main.IdeaManagerWindow.gameObject.SetActive(true);
   */
        UIController.main.IdeaManagerWindow.gameObject.SetActive(true);

        if (talkingNPC.Question != null )
        {
            yield return ShowNPCDialogue(talkingNPC.Question);
        }
    }
    public IEnumerator ShowNPCDialogue(DialogueLineSO Dialogue)
    {
        if (talkingNPC != null)
        {
            ChangeCharacter(talkingNPC.CharacterFile);
            EmoteCharacter(DialogueLineSO.CharacterEmotion.normal);
            EmoteCharacter(DialogueLineSO.CharacterEmotion.normal);

            string label = Dialogue.GetDialogueLine();
            if (label.Length > 0)
            {
                yield return TypeDialog(label);
                float Wait = GetWaitValue(label);
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
