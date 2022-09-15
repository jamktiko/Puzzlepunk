using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    CameraBounds currentBounds;
    public static CameraController main;
    private void Awake()
    {
        main = this;
    }
    public void ForceBounds(CameraBounds newbounds)
    {
        currentBounds = newbounds;
    }
    private void Update()
    {
        transform.position = PlayerMovement.main.transform.position;
    }
}
