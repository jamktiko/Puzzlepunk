using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCanvasController : MonoBehaviour
{
    public string sortingLayer;
    public Canvas mCanvas;
    private void Awake()
    {
        if (mCanvas!=null)
        {
            mCanvas = GetComponent<Canvas>();   
        }
    }
    private void OnEnable()
    {
        if (mCanvas != null)
        {
            mCanvas.worldCamera = Camera.main;
            mCanvas.sortingLayerName = sortingLayer;
        }
    }
}
