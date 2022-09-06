using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement main;
    public float MovementSpeed = 1f;
    private void Awake()
    {
        main = this;
    }
    private void Update()
    {
        HandleMovement();
    }
    public void IssueMoveOrder(Vector2 destination)
    {
        Stop();
        walkPath = Pathfinder.SolvePath(GridNav.main, transform.position, destination);
    }
    public void IssueMoveOrder(InteractableBase interactable)
    {
        Stop();
        InteractObject = interactable;
    }
    void Stop()
    {
        InteractObject = null;
        walkPath = null;
    }
    InteractableBase InteractObject;
    Pathfinder.PathfinderPath walkPath;
    void HandleMovement()
    {
        if (InteractObject!=null)
        {
            if ((transform.position - InteractObject.transform.position).sqrMagnitude <= InteractObject.InteractRange * InteractObject.InteractRange)
            {
                InteractObject.OnInteract();
                Stop();
            }
            else if (walkPath == null)
            {
                walkPath = Pathfinder.SolvePath(GridNav.main, transform.position, InteractObject.transform.position);
            }
        }
        if (walkPath != null)
        {
            if (walkPath.FailureType != Pathfinder.Failure.success)
            {
                Debug.Log(walkPath.FailureType);
                Stop();
            }
            else {
                Vector2 moveDestination = walkPath.Current().worldPos;
                Vector2 delta = (Vector2)transform.position - moveDestination;
                if (delta.sqrMagnitude > MovementSpeed * Time.deltaTime)
                {
                    facesRight = transform.position.x < moveDestination.x;
                    transform.position = Vector3.MoveTowards(transform.position, moveDestination, MovementSpeed * Time.deltaTime * (IsSprinting() ? SprintMultiplier : 1));
                }
                else
                {
                    if (walkPath.Solved())
                        walkPath = null;
                    else
                        walkPath.Next();
                }
            }
        }
    }
    bool facesRight = true;
    public bool IsFacingRight()
    {
        return facesRight;
    }
    public bool IsWalking()
    {
        return walkPath != null;
    }
    public float SprintMultiplier = 2f;
    bool IsSprinting()
    {
        return Input.GetMouseButton(0);
    }
}
