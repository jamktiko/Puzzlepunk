using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimations : MonoBehaviour
{
    public Animator animator;

    private void Update()
    {
        animator.SetBool("walking", PlayerMovement.main.IsWalking());
        animator.SetBool("faceright", PlayerMovement.main.IsFacingRight());
    }
}
