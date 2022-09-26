using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    int originalOrder = 0;
    public float DeltaY;
    public SpriteRenderer spriteRenderer;
    public int deltaOrder = 5;

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawCube(transform.position + Vector3.up * DeltaY, new Vector3(transform.localScale.x * GetComponent<SpriteRenderer>().sprite.rect.size.x / GetComponent<SpriteRenderer>().sprite.pixelsPerUnit, .03f,.03f));
    }
    private void Awake()
    {
        if (spriteRenderer==null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
            originalOrder = spriteRenderer.sortingOrder;
    }


    void Start()
    {
        if (spriteRenderer == null) return;

        spriteRenderer.sortingOrder = Mathf.RoundToInt( -transform.localPosition.y * 10f);
                    
              
        
    }
}
