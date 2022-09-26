using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomTransitioner : MonoBehaviour
{

    public static RoomTransitioner main;
    private void Awake()
    {
        main = this;
    }

    RoomComponent CurrentRoom;
    public void TransitionRoom(string newRoomName, Vector2 pointPosition)
    {
        if (LevelController.main != null && LevelController.main.GetRoomByName(newRoomName) != null)
        {
            TransitionRoom(LevelController.main.GetRoomByName(newRoomName), pointPosition);
        }
    }
    public void TransitionRoom(RoomComponent newRoom, Vector2 pointPosition)
    {
        CurrentRoom = newRoom;
        PlayerMovement.main.Stop();
        PlayerMovement.main.grid = newRoom.grid;
        transform.position = newRoom.transform.position + (Vector3)pointPosition;
        CameraController.main.SetBounds(newRoom.bounds);

    }
}
