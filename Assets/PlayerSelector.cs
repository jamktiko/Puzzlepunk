using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSelector : MonoBehaviour
{
    public static PlayerSelector main;
    public List<PlayerController> characters;
    int selection = -1;
    void Awake()
    {
        main = this;
        characters = new List<PlayerController>();
    }
    public void RegisterCharacter(PlayerController player)
    {
        characters.Add(player);
        if (selection < 0)
        {
            SelectCharacter(0);
        }
    }
    public void DeRegisterCharacter(PlayerController player)
    {
        characters.Remove(player);
    }
    void Update()
    {
        HandlePlayerSwitch();
        UpdateDisplay();
    }
    void UpdateDisplay()
    {
        if (selection >= 0)
            transform.position = characters[selection].transform.position + Vector3.up * 1.5f;
    }
    void HandlePlayerSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            CycleCharacters(true);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            CycleCharacters(false);
        }
    }
    void CycleCharacters(bool left)
    {
        int isel = selection;
        if (left)
        {
            isel--;
            if (isel<0)
            {
                isel += characters.Count;
            }
        }
        else
        {
            isel++;
            if (isel >= characters.Count)
            {
                isel = 0;
            }
        }
        SelectCharacter(isel);
    }
    void SelectCharacter(int nCharacter)
    {
        if (selection >= 0)
        {
            characters[selection].SetSelected(false);
        }
        characters[nCharacter].SetSelected(true);
        selection = nCharacter;
    }
}
