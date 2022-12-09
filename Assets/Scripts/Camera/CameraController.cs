using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    Camera mCam;
    CameraBounds currentBounds;
    Rect rBounds;
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
        if (newbounds!=null)
        rBounds = newbounds.GetBounds();
    }
    private void Update()
    {
        if (PlayerMovement.main != null)
        MovePosition(PlayerMovement.main.transform.position);
    }
    void MovePosition(Vector3 center)
    {
        if (currentBounds == null || mCam == null) return;
        if (width * 2 > rBounds.width)
        {
            center.x = rBounds.center.x;
        }
        else
        {
            center.x = Mathf.Clamp(center.x, rBounds.xMin + width, rBounds.xMax - width);
        }
        if (height * 2 > rBounds.height)
        {
            center.y = rBounds.center.y;
        }
        else
        {
            center.y = Mathf.Clamp(center.y, rBounds.yMin + height, rBounds.yMax - height);
        }
        transform.position = GridSnap(center);
    }
    Vector3 GridSnap(Vector3 main)
    {
        float scaleVal = 100;
        main.x = Mathf.Round(main.x * scaleVal) / scaleVal;
        main.y = Mathf.Round(main.y * scaleVal) / scaleVal;
        return main;
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
