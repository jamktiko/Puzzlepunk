using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera mCam;
    CameraBounds currentBounds;
    public static CameraController main;
    private void Awake()
    {
        main = this;
        if (mCam==null)
            mCam = GetComponent<Camera>();
        Readjust();
    }
    public void SetBounds(CameraBounds newbounds)
    {
        currentBounds = newbounds;
    }
    private void Update()
    {
        if (PlayerMovement.main != null)
        MovePosition(PlayerMovement.main.transform.position);
    }
    void MovePosition(Vector3 center)
    {
        if (currentBounds == null || mCam == null) return;
        if (width * 2 > currentBounds.rBounds.width)
        {
            center.x = currentBounds.rBounds.center.x;
        }
        else
        {
            center.x = Mathf.Clamp(center.x, currentBounds.rBounds.xMin + width, currentBounds.rBounds.xMax - width);
        }
        if (height * 2 > currentBounds.rBounds.height)
        {
            center.y = currentBounds.rBounds.center.y;
        }
        else
        {
            center.y = Mathf.Clamp(center.y, currentBounds.rBounds.yMin + height, currentBounds.rBounds.yMax - height);
        }
        transform.position = center;
    }
    float width = 0;
    float height = 0;
    void Readjust()
    {
        if (mCam == null) return;
        height = mCam.orthographicSize;
        width = height * mCam.aspect;
    }
}
