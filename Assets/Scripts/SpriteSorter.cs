using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    int originalOrder = 0;
    public float DeltaY;
    public SpriteRenderer spriteRenderer;
    public int deltaOrder = 0;

    private void OnDrawGizmosSelected()
    {
        if (spriteRenderer == null) return;
            Gizmos.color = Color.gray;
        Gizmos.DrawCube(transform.position + Vector3.up * DeltaY, new Vector3(transform.localScale.x * spriteRenderer.sprite.rect.size.x / spriteRenderer.sprite.pixelsPerUnit, .05f,.05f));
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
        Sort(-transform.localPosition.y);
    }
    public void Sort(float delta)
    {
        if (spriteRenderer != null)
            spriteRenderer.sortingOrder = Mathf.RoundToInt(delta * 20f);
    }
}
