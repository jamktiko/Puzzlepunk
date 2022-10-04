using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractions : MonoBehaviour
{
    public static PlayerInteractions main;
    private void Awake()
    {
        main = this;
    }
    public float InteractDistance = 1f;
    private void Update()
    {
        HandleInteractCommand();

    }
    public void HandleInteractCommand()
    {
        if (PlayerMovement.main.CanAct() && Input.GetKeyDown(KeyCode.X))
            PlayerInteract();
    }

    public void PlayerInteract()
    {
        foreach (RaycastHit2D hit in Physics2D.CircleCastAll(transform.position, .1f, PlayerMovement.main.facing, InteractDistance))
        {
            if (hit.transform.TryGetComponent(out InteractableBase interactable))
            {

                interactable.OnInteract();
                return;
            }
        }
    }
    
    public bool CanInteract = false;
    public void UpdateCanInteract()
    {
        CanInteract = CheckInteractables();
    }
    bool CheckInteractables()
    {
        foreach (RaycastHit2D hit in Physics2D.CircleCastAll(transform.position, .1f, PlayerMovement.main.facing, InteractDistance))
        {
            if (hit.transform.TryGetComponent(out InteractableBase interactable))
            {

                return true;
            }
        }
        return false;
    }
}
