using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorter : MonoBehaviour
{
    public enum SortingMode
    {
        AlwaysBehind,
        Dynamic,
        AlwaysInFront,
    }
    public SortingMode Orientation;
    public float DeltaY;
    public SpriteRenderer spriteRenderer;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawCube(transform.position + Vector3.up * DeltaY, new Vector3(transform.localScale.x * spriteRenderer.sprite.rect.size.x / spriteRenderer.sprite.pixelsPerUnit, .03f,.03f));
    }
    private void Awake()
    {
        if (spriteRenderer==null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        if (spriteRenderer == null || PlayerAnimations.main == null) return;

        switch (Orientation)
        {
            case SortingMode.AlwaysInFront:
                spriteRenderer.sortingOrder = PlayerAnimations.main.spriteRenderer.sortingOrder + 1;
                break;
            case SortingMode.AlwaysBehind:
                spriteRenderer.sortingOrder = PlayerAnimations.main.spriteRenderer.sortingOrder - 1;
                break;
            case SortingMode.Dynamic:
                if (PlayerMovement.main!=null)
                {
                    if (PlayerMovement.main.transform.position.y > transform.position.y + DeltaY)
                    {
                        spriteRenderer.sortingOrder = PlayerAnimations.main.spriteRenderer.sortingOrder + 1;
                    }
                    else
                    {
                        spriteRenderer.sortingOrder = PlayerAnimations.main.spriteRenderer.sortingOrder - 1;
                    }
                }
                break;
        }
    }
}
