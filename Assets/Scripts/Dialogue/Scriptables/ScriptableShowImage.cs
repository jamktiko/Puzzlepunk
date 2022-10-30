using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[CreateAssetMenu(fileName = "Image", menuName = "Dialogue/Components/Image")]
public class ScriptableShowImage : ScriptableDialogue
{
    public bool HideText = false;
    public Sprite DialogueImage;
    public override IEnumerator Run(DialogueUIController DC)
    {
        if (HideText)
            DC.HideDialogue();
        if (DialogueImage != null)
        {
            DC.dialogImage.sprite = DialogueImage;
            DC.dialogImage.color = Color.white;
            DC.dialogImage.enabled = true;
        }        
        yield return DC.PostLineWait();
        OnSkipped(DC);
    }
}
