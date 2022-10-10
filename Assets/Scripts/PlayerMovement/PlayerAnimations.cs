using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public static PlayerAnimations main;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public SpriteSorter zSorter;

    private void Awake()
    {
        main = this;
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        if (zSorter == null)
        {
            zSorter = GetComponent<SpriteSorter>();
        }
    }
    private void Update()
    {
        if (PlayerMovement.main.CanAct())
        {
            bool Walking = PlayerMovement.main.moveInput.x != 0 || PlayerMovement.main.moveInput.y != 0;
            SetWalking(Walking);
            if (Walking)
            {
                animator.SetBool("Going Left", PlayerMovement.main.moveInput.x < 0);
                animator.SetBool("Going Right", PlayerMovement.main.moveInput.x > 0);

                animator.SetBool("Going Front", PlayerMovement.main.moveInput.y < 0);
                animator.SetBool("Going Back", PlayerMovement.main.moveInput.y > 0);
            }

            SetCurious(PlayerInteractions.main.CanInteract);
        }

        if (zSorter != null)
            zSorter.Sort(PlayerTransitionController.main.CurrentRoom.transform.position.y - transform.position.y );
    }
    public void SetWalking(bool value)
    {
        animator.SetBool("Is Walking", value);
    }
    public void SetCurious(bool value)
    {
        animator.SetBool("InteractPrompt", value);
    }
    public void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }
}
