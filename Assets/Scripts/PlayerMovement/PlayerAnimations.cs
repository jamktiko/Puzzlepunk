using System.Collections;
using System.Collections.Generic;
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
    private void Start()
    {
        PlayAnimation("stand_up");
    }
    private void Update()
    {
        animator.SetBool("walking", PlayerMovement.main.IsWalking());
        animator.SetBool("faceright", PlayerMovement.main.IsFacingRight());


        if (zSorter != null)
            zSorter.Sort(PlayerTransitionController.main.CurrentRoom.transform.position.y - transform.position.y );
    }
    public void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }
}
