using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public Animator animator;

    private void Awake()
    {
        if (animator == null)
        {
            animator = GetComponent<Animator>();
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
