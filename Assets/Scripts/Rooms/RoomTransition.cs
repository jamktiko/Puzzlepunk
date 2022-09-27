using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransition : MonoBehaviour
{
    public RoomComponent destinationRoom;
    public Vector2 destination;

    public void TriggerTransition()
    {
        PlayerTransitionController.main.TransitionRoom(destinationRoom,destination);
    }

    void OnDrawGizmosSelected()
    {
        if (destinationRoom != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(destinationRoom.transform.position + (Vector3)destination, .33f);
        }
    }
}
