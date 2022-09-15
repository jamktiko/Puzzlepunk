using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    public Rect rBounds = new Rect (0,0,0,0);
    private void Start()
    {
        rBounds = new Rect(transform.position.x - transform.localScale.x * .5f, transform.position.y - transform.localScale.y * .5f, transform.localScale.x, transform.localScale.y);
    }
}
