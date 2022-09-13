using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnifyingGlassController : MonoBehaviour
{
    public float MagnifyingZoom = 2;
    public GameObject PlayerCursor;
    public GameObject ZoomedInImage;

    private void Start()
    {
        if (PlayerCursor != null)
        {
            ZoomedInImage.transform.localScale = Vector3.one * MagnifyingZoom;
        }
    }

    private void Update()
    {
        if (PlayerCursor!=null)
        {
            PlayerCursor.transform.position = Input.mousePosition;

            RectTransform rect = ZoomedInImage.GetComponent<RectTransform>();
            ZoomedInImage.transform.position = new Vector3(Screen.width, Screen.height) -Input.mousePosition;
        }
    }
}
