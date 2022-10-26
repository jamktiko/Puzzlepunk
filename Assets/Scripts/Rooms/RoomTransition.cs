using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    public RoomComponent destinationRoom;
    public Vector2 destination;
    public bool instant;
    public PlayerMovement.Orientation SetOrientation = PlayerMovement.Orientation.down;

    public void TriggerTransition()
    {
        PlayerTransitionController.main.TransitionRoom(destinationRoom,destination, SetOrientation, instant);
    }

    void OnDrawGizmosSelected()
    {
        if (destinationRoom != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(destinationRoom.transform.position + (Vector3)destination, .06f);
        }
    }
}
