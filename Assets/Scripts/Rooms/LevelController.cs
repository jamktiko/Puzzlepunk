using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public static LevelController main;
     RoomComponent[] rooms;   
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
    public RoomComponent GetRoomByPoint(Vector2 point)
    {
        foreach (RoomComponent r in rooms)
        {
            if (r.grid.GetNodeAt(point, out GridNav.Node n))
                return r;
        }
        return null;
    }

    public NavPoint StartingPoint;
    void InitPlayer()
    {
        if (StartingPoint != null && PlayerTransitionController.main!=null)
        {
            PlayerTransitionController.main.TeleportToPoint(StartingPoint, PlayerMovement.Orientation.down, true);
        }
        }
}
