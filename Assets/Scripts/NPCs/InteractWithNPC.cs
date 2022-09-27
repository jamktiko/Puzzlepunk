using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractWithNPC : MonoBehaviour
{
    public CharacterSO myNPC;
    public  void OnInteract()
    {
        UIController.main.dialogueController.TalkWithNPC(myNPC);
    }

}
