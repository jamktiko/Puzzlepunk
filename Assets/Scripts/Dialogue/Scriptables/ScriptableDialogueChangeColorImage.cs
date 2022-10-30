using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "Image Color", menuName = "Dialogue/Components/Image Color")]
public class ScriptableDialogueChangeColorImage : ScriptableShowImage
{
    public Color StartColor = Color.white;
    public float Duration = 1;
    public Color FinalColor = Color.white;
    public override IEnumerator Run(DialogueUIController DC)
    {
        if (HideText)
            DC.HideDialogue();
        DC.dialogImage.enabled = true;
        DC.dialogImage.sprite = DialogueImage;

        Color currentColor = StartColor;

        for (float time = 0; time < Duration; time += Time.deltaTime)
        {
            DC.dialogImage.color = currentColor;

            float progress = time / Duration;

            currentColor.r = Mathf.Lerp(StartColor.r, FinalColor.r, progress);
            currentColor.g = Mathf.Lerp(StartColor.g, FinalColor.g, progress);
            currentColor.b = Mathf.Lerp(StartColor.b, FinalColor.b, progress);
            currentColor.a = Mathf.Lerp(StartColor.a, FinalColor.a, progress);

            yield return new WaitForEndOfFrame();
        }
        DC.dialogImage.color = FinalColor;
        DC.dialogImage.enabled = false;
        OnSkipped(DC);
    }
    public override void OnSkipped(DialogueUIController DC)
    {
        base.OnSkipped(DC);
        DC.dialogImage.enabled = false;
        DC.dialogImage.color = Color.white;
    }
}
