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

    public RoomComponent CurrentRoom;
    public void TransitionRoom(string newRoomName, Vector2 pointPosition)
    {
        if (LevelController.main != null && LevelController.main.GetRoomByName(newRoomName) != null)
        {
            TransitionRoom(LevelController.main.GetRoomByName(newRoomName), pointPosition);
        }
    }
    public void TransitionRoom(RoomComponent newRoom, Vector2 pointPosition)
    {
        if (CurrentRoom!=null)
            CurrentRoom.gameObject.SetActive(false);

        CurrentRoom = newRoom;

        if (PlayerMovement.main != null)
        {
            PlayerMovement.main.Stop();
            PlayerMovement.main.grid = newRoom.grid;
        }
        transform.position = newRoom.transform.position + (Vector3)pointPosition;

        if (newRoom.bounds!=null)
        CameraController.main.SetBounds(newRoom.bounds);
        CurrentRoom.gameObject.SetActive(true);
    }
}
