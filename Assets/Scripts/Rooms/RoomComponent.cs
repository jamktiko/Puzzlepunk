using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RoomComponent : MonoBehaviour
{
    public GridNav grid;
    public CameraBounds bounds;
    private void Awake()
    {
        if (grid == null)
            grid = GetComponentInChildren<GridNav>();
        if (bounds == null)
            bounds = GetComponentInChildren<CameraBounds>();
    }
}
