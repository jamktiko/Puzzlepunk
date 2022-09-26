using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        foreach (RoomComponent room in rooms)
        {
            room.gameObject.SetActive(false);
        }
    }
    private void Start()
    {
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
        if (StartingRoom != null && RoomTransitioner.main!=null)
        {
            RoomTransitioner.main.TransitionRoom(StartingRoom, StartingPosition);
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
