using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static LevelController main;
    RoomComponent[]
rooms;   
    NavPoint    [] navPoints;
    void Awake()
    {
        main = this;
        navPoints = GetComponentsInChildren<NavPoint>();
        rooms = GetComponentsInChildren<RoomComponent>();
    }
    private void Start()
    {
        foreach (RoomComponent room in rooms)
        {
            room.gameObject.SetActive(false);
        }
        InitPlayer();
    }

    public NavPoint GetPointByName(string rName)
    {
        foreach (NavPoint p in navPoints)
        {
            if (p.PointID == rName)
                return p;
        }
        return null;
    }
    public RoomComponent GetRoomByName(string rName)
    {
        foreach (RoomComponent r in rooms)
        {
            if (r.name == rName)
                return r;
        }
        return null;
    }

    public RoomComponent StartingRoom;
    public Vector2 StartingPosition;
    void InitPlayer()
    {
        if (StartingRoom != null && PlayerTransitionController.main!=null)
        {
            PlayerTransitionController.main.TransitionRoom(StartingRoom, StartingPosition,true);
        }
        }
    void OnDrawGizmosSelected()
    {
        if (StartingRoom != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(StartingRoom.transform.position + (Vector3)StartingPosition, .1f * transform.lossyScale.x);
        }
    }
}
