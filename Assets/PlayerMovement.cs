using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CapsuleCollider2D))]
public class PlayerMovement : MonoBehaviour, IMoveable
{
    public bool HasControl = false;
    bool FacesRight = false;
    public float WalkSpeed = 10;
    public float JumpTime = .33f;
    public float MaxJumpSpeed = 2;
    public float JumpSpeed = 1;
    public float MaxFallSpeed = 1;
    public float MaxGlideSpeed = 1;
    public float GroundTime = .1f;

    Rigidbody2D rbody;
    CapsuleCollider2D collider;
    private void Awake()
    {
        rbody = GetComponent<Rigidbody2D>();
        collider = GetComponent<CapsuleCollider2D>();
    }
    float LastGroundTime = 0;
    public bool IsGrounded()
    {
        return LastGroundTime > Time.time;
    }
    Vector2 modifiedVelocity;
    private void OnCollisionStay2D(Collision2D collision)
    {
        foreach (ContactPoint2D point in collision.contacts)
        {
            if (IsAbove(point.point.y))
            {
                LastGroundTime = Time.time + GroundTime;
            }
        }
    }
    void Update()
    {
        modifiedVelocity = rbody.velocity;
        if (HasControl)
        {
            parent.Move(Input.GetAxis("Horizontal"));
            parent.TryJump();
        }
        else
        {
            parent.Move(0);
        }
        HandleFall();
        rbody.velocity = modifiedVelocity;
    }
    public void Move(float dir)
    {
        modifiedVelocity.x = dir* WalkSpeed;
        if (dir != 0 && IsGrounded())
        {
            SetFacing(dir > 0);
        }
    }
    public void TryJump()
    {
        if (IsGrounded())
        {
            if (Input.GetKeyDown(KeyCode.Space) && JumpCoroutine == null)
            {
                LastGroundTime = 0;
                JumpCoroutine = StartCoroutine(JumpFloat());
            }
        }
        
    }
    void HandleFall()
    {
        if (!IsGrounded())
        {
            if (Input.GetKey(KeyCode.Space))
            {
                if (modifiedVelocity.y < -MaxGlideSpeed)
                {
                    modifiedVelocity.y = -MaxGlideSpeed;
                }
            }
            else if (modifiedVelocity.y < -MaxFallSpeed)
            {
                modifiedVelocity.y = -MaxFallSpeed;
            }
        }
    }
    Coroutine JumpCoroutine;
    IEnumerator JumpFloat()
    {
        float jumpEndTime = Time.time + JumpTime;

        while (jumpEndTime >= Time.time && Input.GetKey(KeyCode.Space))
        {
            rbody.velocity = rbody.velocity * Vector2.right + Vector2.up * JumpSpeed;
            yield return new WaitForFixedUpdate();
        }
        
            rbody.velocity = rbody.velocity * Vector2.right + Vector2.up * MaxJumpSpeed;
        JumpCoroutine = null;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public void MovePosition(Vector3 newPos)
    {
        rbody.position = newPos;
    }

    public void MoveDirection(Vector3 direction)
    {
        MovePosition(transform.position + direction);
    }

    public void SetFacing(bool right)
    {
        FacesRight = right;
        transform.localScale = new Vector3(right ? 1 : -1, 1, 1);
    }

    public bool IsAbove(float y)
    {
        return transform.position.y + (collider.size.y + collider.size.x * .66f) * .5f > y;
    }
    public Vector2 GetForward()
    {
        return Vector2.right * (FacesRight ? 1 : -1);
    }
}
