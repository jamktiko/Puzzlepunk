using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DialogueScriptSO;

[CreateAssetMenu(fileName = "Audio Line", menuName = "Dialogue/Components/Audio Line")]
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
    public override IEnumerator Run(DialogueUIController DC)
    {
            if (PlayType == DialogueAudioSO.AudioPlayType.before)
            {
                if (AudioClip != null)
                DC.PlaySound(AudioClip);
            }
            string line = GetDialogueLine();
            if (line.Length > 0)
            {
                yield return DC.TypeDialog(line);
                if (PlayType == DialogueAudioSO.AudioPlayType.after)
                {
                    if (AudioClip != null)
                    DC.PlaySound(AudioClip);
                }
                yield return DC.PostLineWait(line);
            }
    }
}
