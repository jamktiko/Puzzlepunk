using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClueComponent : MonoBehaviour
{
    public string ClueID = ""; 
    public void Reveal()
    {
        if (ClueID == "" || ClueID == "-")
        {
            Debug.LogError("Interactable " + name + " has no value assigned!");
        }
        else
        {
            PlayerClueController.main.RevealClue(ClueID, true);
        }
    }
}
