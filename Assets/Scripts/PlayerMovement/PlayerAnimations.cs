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
        animator.SetBool("Is Walking", PlayerMovement.main.IsWalking());
        animator.SetBool("Going Left", PlayerMovement.main.facing == PlayerMovement.Direction.left);
        animator.SetBool("Going Right", PlayerMovement.main.facing == PlayerMovement.Direction.right);
        animator.SetBool("Going Front", PlayerMovement.main.facing == PlayerMovement.Direction.down);
        animator.SetBool("Going Back", PlayerMovement.main.facing == PlayerMovement.Direction.up);


        if (zSorter != null)
            zSorter.Sort(PlayerTransitionController.main.CurrentRoom.transform.position.y - transform.position.y );
    }
    public void PlayAnimation(string animationName)
    {
        animator.Play(animationName);
    }
}
