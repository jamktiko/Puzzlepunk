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


        spriteRenderer.sortingOrder = Mathf.RoundToInt((RoomTransitioner.main.CurrentRoom.transform.position.y - transform.position.y) * 10f);
    }
    public void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }
}
