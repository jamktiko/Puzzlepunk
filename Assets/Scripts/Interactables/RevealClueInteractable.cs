using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RevealClueInteractable : InteractableBase
{
    public string ClueID = "";
    public override void OnInteract()
    {
        if (ClueID == "" || ClueID == "-")
        {
            Debug.LogError("Interactable " + name + " has no value assigned!");
        }
        else
        {
            PlayerClueController.main.RevealClue(ClueID, true);
        }
        base.OnInteract();
    }
}
