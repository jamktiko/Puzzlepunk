using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithNPC : InteractableBase
{
    public CharacterSO myNPC;
    public override void OnInteract()
    {
        UIController.main.dialogueController.TalkWithNPC(myNPC);
        base.OnInteract();
    }

}
