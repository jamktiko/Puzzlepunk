using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class CameraBounds : MonoBehaviour
{
    public Rect rBounds = new Rect (0,0,0,0);
    private void OnValidate()
    {
        rBounds = new Rect(transform.position.x - transform.localScale.x * .5f, transform.position.y - transform.localScale.y * .5f, transform.localScale.x, transform.localScale.y);
    }
    /*private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawCube(new Vector3(rBounds.center.x,rBounds.center.y,0),new Vector3(rBounds.width,rBounds.height,1));
    }*/
}
