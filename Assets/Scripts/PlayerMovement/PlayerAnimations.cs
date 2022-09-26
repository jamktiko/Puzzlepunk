using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public static PlayerAnimations main;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

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
    }
    private void Start()
    {
        PlayAnimation("stand_up");
    }
    private void Update()
    {
        animator.SetBool("walking", PlayerMovement.main.IsWalking());
        animator.SetBool("faceright", PlayerMovement.main.IsFacingRight());
    }
    public void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }
}
