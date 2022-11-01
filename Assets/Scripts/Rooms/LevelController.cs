using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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

        Reactors = new List<VariableReactionChange>();
        Reactors.AddRange(GetComponentsInChildren<VariableReactionChange>());
    }
    private void Start()
    {
        foreach (RoomComponent room in rooms)
        {
            room.gameObject.SetActive(false);
        }
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
            if (r.grid.TryGetWorldNodeAt(point, out GridNav.Node n))
                return r;
        }
        return null;
    }
    public UnityEvent OnGameStarted;
    public void BeginGame()
    {
        OnGameStarted.Invoke();
    }
    #region Variable Reactors

    public List<VariableReactionChange> Reactors = new List<VariableReactionChange>();

    #endregion
}
