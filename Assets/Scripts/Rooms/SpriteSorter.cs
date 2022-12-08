using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorter : MonoBehaviour, IOnMoved
{
    int originalOrder = 0;
    public float DeltaY;
    public bool AlwaysUpdate = false;
    public SpriteRenderer spriteRenderer;
    public int deltaOrder = 0;

    float unit = .02f;

    private void OnDrawGizmosSelected()
    {
        if (spriteRenderer == null) return;
            Gizmos.color = Color.gray;
        Gizmos.DrawCube(transform.position + Vector3.up * DeltaY, new Vector3(transform.localScale.x * spriteRenderer.sprite.rect.size.x / spriteRenderer.sprite.pixelsPerUnit, unit, unit));
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
        OnMove();
    }
    void Update()
    {
        if (AlwaysUpdate)
            OnMove();
    }
    public void ToggleAlwaysUpdate(bool value)
    {
        AlwaysUpdate = value;
    }
    public void OnMove()
    {
        Sort(-transform.localPosition.y - DeltaY);
    }
    public void Sort(float delta)
    {
        if (spriteRenderer != null)
            spriteRenderer.sortingOrder = Mathf.RoundToInt((delta) / unit) + deltaOrder;
    }
}
