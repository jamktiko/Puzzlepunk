
using UnityEngine;


public interface IMoveable
{
    public Vector3 GetPosition();
    public void MovePosition(Vector3 newPos);
    public void MoveDirection(Vector3 direction);
    public bool IsAbove(float y);
    public void SetFacing(bool right);
    public bool IsGrounded();
}