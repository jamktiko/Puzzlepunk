using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransitioner : MonoBehaviour
{
    public RoomComponent StartingRoom;
    public Vector2 StartingPosition;
    RoomComponent CurrentRoom;

    private void Start()
    {
        TransitionRoom(StartingRoom, StartingPosition);
    }

    public void TransitionRoom(RoomComponent newRoom, Vector2 pointPosition)
    {
        CurrentRoom = newRoom;
        transform.position = newRoom.transform.position + (Vector3)pointPosition;
        GridNav.main = newRoom.grid;
    }

    void OnDrawGizmosSelected()
    {
        if (StartingRoom != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(StartingRoom.transform.position + (Vector3)StartingPosition, .33f);
        }
    }
}
