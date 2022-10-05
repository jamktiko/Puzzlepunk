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
    public void TeleportToPoint(string pointName, bool skipCoroutine)
    {
        if (LevelController.main != null && LevelController.main.GetPointByName(pointName) != null)
        {
            TeleportToPoint( LevelController.main.GetPointByName(pointName), skipCoroutine);
        }
    }
    public void TeleportToPoint(NavPoint roomPoint, bool skipCoroutine)
    {
        if (roomPoint!=null)
        {
            roomPoint.FindRoom();
        }
            if (roomPoint.room != null)
            {
                TransitionRoom(roomPoint.room, roomPoint.GetRelativePosition(), skipCoroutine);
            }
        
    }
    public void TransitionRoom(string newRoomName, Vector2 pointPosition, bool skipCoroutine)
    {
        if (LevelController.main != null && LevelController.main.GetRoomByName(newRoomName) != null)
        {
            TransitionRoom(LevelController.main.GetRoomByName(newRoomName), pointPosition, skipCoroutine);
        }
    }
    Coroutine TransitionCoroutine;
    public void TransitionRoom(RoomComponent newRoom, Vector2 pointPosition, bool skipCoroutine)
    {

        if (skipCoroutine)
        {
            MovePlayerToNewRoom(newRoom, pointPosition);
            return;
        }
        else if (newRoom == CurrentRoom)
            return;
        if (TransitionCoroutine!=null)
        {
            StopCoroutine(TransitionCoroutine);
        }
        TransitionCoroutine = StartCoroutine(FadeCoroutine(newRoom, pointPosition));
    }
    public IEnumerator FadeCoroutine(RoomComponent newRoom, Vector2 pointPosition)
    {
        if (UIController.main.TransitionScreen!=null)
        {
            UIController.main.TransitionScreen.StopAllCoroutines();
            yield return UIController.main.TransitionScreen.AwaitTransitionIn(ScreenTransitionTime);
        }
        MovePlayerToNewRoom(newRoom, pointPosition);
        if (UIController.main.TransitionScreen != null)
        {
            yield return UIController.main.TransitionScreen.AwaitTransitionOut(ScreenTransitionTime);
        }
    }
    void MovePlayerToNewRoom(RoomComponent newRoom, Vector2 pointPosition)
    {
        if (CurrentRoom != null)
            CurrentRoom.gameObject.SetActive(false);

        CurrentRoom = newRoom;

        if (PlayerMovement.main != null)
        {
            PlayerMovement.main.Stop();
            PlayerMovement.main.grid = newRoom.grid;
        }

        Vector3 teleportPosition = newRoom.transform.position + (Vector3)pointPosition;

        var point = newRoom.grid.GetClosestToPoint(newRoom.grid.TranslateCoordinate(teleportPosition));

        transform.position = point.worldPos;

        if (CurrentRoom.bounds != null)
            CameraController.main.SetBounds(CurrentRoom.bounds);
        CurrentRoom.gameObject.SetActive(true);
    }
}
