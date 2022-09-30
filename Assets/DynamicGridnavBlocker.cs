using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicGridnavBlocker : MonoBehaviour
{
    public GridNav grid;
    public Collider2D collider;
    List<GridNav.Node> blockedNodes = new List<GridNav.Node>();

    bool initialized = false;
    void Initialize()
    {
        if (!initialized)
        {
            initialized = true;

            if (collider == null)
            {
                collider = GetComponent<Collider2D>();
            }
            if (grid == null)
            {
                grid = GetComponentInParent<GridNav>();
            }
        }
    }

    private void Start()
    {
        Initialize();
    }
    private void OnEnable()
    {
        if (initialized)
            BlockNodes(transform.position);
    }
    private void OnDisable()
    {
        ClearNodes();
    }
    void ClearNodes()
    {
        foreach (GridNav.Node node in blockedNodes)
        {
            node.SetBlocked(false);
        }
        blockedNodes.Clear();
    }
    void BlockNodes(Vector2 position)
    {
        if (grid != null)
        {
            if (collider.GetType() == typeof(BoxCollider2D))
            {
                BoxCollider2D BC = (BoxCollider2D)collider;
                blockedNodes.AddRange(grid.GetNodesInBox(position - BC.size * .5f, position + BC.size * .5f));
            }
            else if (collider.GetType() == typeof(CircleCollider2D))
            {
                CircleCollider2D CC = (CircleCollider2D)collider;
                blockedNodes.AddRange(grid.GetNodesInCircle(position , CC.radius));
            }
        }
        foreach (GridNav.Node node in blockedNodes)
        {
            node.SetBlocked(true);
        }
    }
}
