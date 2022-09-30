using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicGridnavBlocker : MonoBehaviour
{
    public GridNav grid;
    List<GridNav.Node> blockedNodes = new List<GridNav.Node>();

    private void Awake()
    {
        
    }
    private void OnEnable()
    {
        BlockNodes(transform.position);
    }
    private void OnDisable()
    {
        UnblockNodes();
    }
    void UnblockNodes()
    {

    }
    void BlockNodes(Vector2 position)
    {

    }
}
