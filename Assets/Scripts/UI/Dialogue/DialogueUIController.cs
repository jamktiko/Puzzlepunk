using System.Collections;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueUIController : MonoBehaviour
{

    [Header("Portrait")]
    DialogueCharacterSO Character;
    public Image Head;
    public Image Face;

    [Header("Components")]

    public int wordsPerSecond = 10;

    [Header("Dialogue Box")]
    public TextMeshProUGUI dialogText;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI skipText;

    [Header("Multiple Choice")]
    public MultipleChoiceContainer MultipleChoice;

    Coroutine CutsceneCoroutine;
    private void Update()
    {
        HandleExit();
    }
    float skipPercent = 0;
    void HandleExit()
    {
        if (CutsceneCoroutine!=null) //TODO define skip key
        {
            if (Input.GetButtonDown("FastForward"))
            {
                SkipLine = true;
            }
            if (Input.GetButton("Skip"))
            {
                skipPercent += Time.deltaTime;
                if (skipText!=null)
                skipText.color = new Color(1, 1, 1, skipPercent);
            }
            else
            {
                skipPercent = 0;
                if (skipText != null)
                    skipText.color = Color.clear;
            }
            if (skipPercent > 1)
            {
                Close();
            }
        }
    }
    public void PlayCutscene(DialogueScriptSO Script)
    {
        if (talkingNPC == null)
        {
            UIController.main.OpenWindow(UIController.UIWindow.dialogue);
        }
        EndDialogue();

        CutsceneCoroutine = StartCoroutine(PlayCutsceneCoroutine(Script));
    }
    public bool IsInDialogueMode()
    {
        return gameObject.activeSelf && (CutsceneCoroutine != null || MultipleChoice.gameObject.activeSelf);
    }
    IEnumerator PlayCutsceneCoroutine(DialogueScriptSO Script)
    {
        HideMultipleChoice();
        int quote = 0;
        while (quote< Script.Dialogue.Length)
        {
            yield return Script.Dialogue[quote].Run(this);
            quote++;
        }
        if (talkingNPC == null)
        {
            Close();
        }
        else
        {
            yield return talkingNPC.Question.Run(this);
        }
    }
    void EndDialogue()
    {
        if (CutsceneCoroutine != null)
            StopCoroutine(CutsceneCoroutine);
        CutsceneCoroutine = null;
    }
    public void Close()
    {
        EndDialogue();
        UIController.main.CloseWindow();
    }
    public void LoadDialogueCharacter(DialogueLineSO NewLine)
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
    public IEnumerator PostLineWait(string line)
    {
            yield return PostLineWait(GetWaitValue(line));        
    }
    public IEnumerator PostLineWait(float Wait)
    {
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
    public float GetWaitValue(string line)
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

    public AudioSource audiosrc;
    public void PlaySound(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (audiosrc != null)
        {
            audiosrc.pitch = pitch;
            audiosrc.PlayOneShot(clip, volume);
        }
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
        EndDialogue();
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
        EndDialogue();

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
            yield return talkingNPC.WelcomeLine.Run(this); 
        }

        if (talkingNPC.Question != null )
        {
            yield return talkingNPC.Question.Run(this);
        }
    }
    #endregion
}
