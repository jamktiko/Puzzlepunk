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
    public float InteractRadius = 1f;
    private void Update()
    {
        HandleInteractCommand();

    }
    public void HandleInteractCommand()
    {
        if (PlayerMovement.main.CanAct() && PlayerInputListener.control.ZoePlayer.Interact.WasPressedThisFrame())
            PlayerInteract();
    }

    public void PlayerInteract()
    {
        foreach (RaycastHit2D hit in Physics2D.CircleCastAll(transform.position, .05f, PlayerMovement.main.facing, InteractDistance))
        {
            if (hit.transform.TryGetComponent(out InteractableBase interactable))
            {
                PlayerAnimations.main.SetWalking(false);
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
        foreach (RaycastHit2D hit in Physics2D.CircleCastAll(transform.position, InteractRadius, PlayerMovement.main.facing, InteractDistance))
        {
            if (hit.transform.TryGetComponent(out InteractableBase interactable))
            {
                if (interactable.CanBeInteractedWith())
                    return true;
            }
        }
        return false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + InteractDistance * (Vector3)(PlayerMovement.main == null ? Vector2.down : PlayerMovement.main.facing), InteractRadius);
    }
}
