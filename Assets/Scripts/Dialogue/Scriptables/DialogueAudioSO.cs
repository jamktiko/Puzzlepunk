using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DialogueScriptSO;

[CreateAssetMenu(fileName = "DialogueAudioSO", menuName = "Dialogue/DialogueAudioSO")]
public class DialogueAudioSO : DialogueLineSO
{
        [Header("Effects")]
        public AudioPlayType PlayType = AudioPlayType.never;
        public AudioClip AudioClip;
        public float DialogueShake;
    public enum AudioPlayType
    {
        never = 0,
        before = 1,
        after = 2
    }

}
