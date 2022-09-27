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
    private void OnDrawGizmosSelected()
    {
        Rect rBounds = GetBounds();
        Gizmos.color = Color.white;
        Gizmos.DrawCube(new Vector3(rBounds.center.x,rBounds.center.y,0),new Vector3(rBounds.width,rBounds.height,1));
    }
}
