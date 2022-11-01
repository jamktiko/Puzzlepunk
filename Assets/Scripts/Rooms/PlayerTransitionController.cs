using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTransitionController : MonoBehaviour
{
    public float ScreenTransitionTime = 1f;
    public static PlayerTransitionController main;
    private void Awake()
    {
        main = this;
    }

    public RoomComponent CurrentRoom;
    public void TeleportToPoint(string pointName, PlayerMovement.Orientation endOrientation, bool skipCoroutine)
    {
        if (LevelController.main != null && LevelController.main.GetPointByName(pointName) != null)
        {
            TeleportToPoint( LevelController.main.GetPointByName(pointName), endOrientation,skipCoroutine);
        }
    }
    public void TeleportToPoint(NavPoint roomPoint, PlayerMovement.Orientation endOrientation, bool skipCoroutine)
    {
        if (roomPoint!=null)
        {
            roomPoint.FindRoom();
        }
        if (roomPoint == null) {
            Debug.LogError("Point not found");
                return;
                }
            if (roomPoint.room != null)
            {
                TransitionRoom(roomPoint.room, roomPoint.GetRelativePosition(), endOrientation, skipCoroutine);
            }
        
    }
    public void TransitionRoom(string newRoomName, Vector2 pointPosition, PlayerMovement.Orientation endOrientation, bool skipCoroutine)
    {
        if (LevelController.main != null && LevelController.main.GetRoomByName(newRoomName) != null)
        {
            TransitionRoom(LevelController.main.GetRoomByName(newRoomName), pointPosition, endOrientation, skipCoroutine);
        }
    }
    Coroutine TransitionCoroutine;
    public void TransitionRoom(RoomComponent newRoom, Vector2 pointPosition, PlayerMovement.Orientation endOrientation, bool skipCoroutine)
    {

        if (skipCoroutine)
        {
            MovePlayerToNewRoom(newRoom, pointPosition);
            PlayerMovement.main.SetOrientation(endOrientation);
            return;
        }
        else if (newRoom == CurrentRoom)
            return;
        if (TransitionCoroutine!=null)
        {
            StopCoroutine(TransitionCoroutine);
        }
        TransitionCoroutine = StartCoroutine(FadeCoroutine(newRoom, endOrientation, pointPosition));
    }
    public IEnumerator FadeCoroutine(RoomComponent newRoom, PlayerMovement.Orientation endOrientation, Vector2 pointPosition)
    {
        PlayerCinematicController.main.SetCinematicMode(true, false);
        if (UIController.main.TransitionScreen!=null)
        {
            UIController.main.TransitionScreen.StopAllCoroutines();
            yield return UIController.main.TransitionScreen.AwaitTransitionIn(ScreenTransitionTime);
        }
        MovePlayerToNewRoom(newRoom, pointPosition);
        PlayerMovement.main.SetOrientation(endOrientation);
        if (UIController.main.TransitionScreen != null)
        {
            yield return UIController.main.TransitionScreen.AwaitTransitionOut(ScreenTransitionTime);
        }
        PlayerCinematicController.main.SetCinematicMode(false, false);
    }
    void MovePlayerToNewRoom(RoomComponent newRoom, Vector2 pointPosition)
    {
        if (CurrentRoom != null)
            CurrentRoom.gameObject.SetActive(false);

        CurrentRoom = newRoom;

        if (PlayerMovement.main != null)
        {
            PlayerMovement.main.movement.Stop();
            PlayerMovement.main.movement.grid = newRoom.grid;
        }

        Vector3 teleportPosition = newRoom.transform.position + (Vector3)pointPosition;
        PlayerMovement.main.movement.JumpToPoint(newRoom.grid.GetClosestToPoint(newRoom.grid.TranslateCoordinate(teleportPosition)));

        if (CurrentRoom.bounds != null)
            CameraController.main.SetBounds(CurrentRoom.bounds);
        CurrentRoom.gameObject.SetActive(true);
    }
}
