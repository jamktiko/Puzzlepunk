using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavPoint : MonoBehaviour
{
    public string PointID;

    public RoomComponent room;
    public void Start()
    {
        if (PointID == "")
            PointID = name;
            FindRoom();
    }
    void FindRoom()
    {
        if (room == null)
            room = LevelController.main.GetRoomByPoint(transform.position);
    }
    public Vector2 GetRelativePosition()
    {
        if (room != null)
            return transform.position - room.transform.position;
        return transform.position;
    }
}
