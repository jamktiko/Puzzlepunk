using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    public Rect GetBounds()
    {
        return new Rect(transform.position.x - transform.localScale.x * .5f, transform.position.y - transform.localScale.y * .5f, transform.localScale.x, transform.localScale.y);
    }
}
