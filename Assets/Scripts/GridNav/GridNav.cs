using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNav : MonoBehaviour
{
    public float UnitSize = 1;
    public float Borders = 1;

    Node[,] Nodes;

    public class Node
    {
        public Vector2Int gridPos;
        public Vector2 worldPos;
         bool passible;
         bool blocked;
        public Node[] neighbors;

        public Node(Vector2Int gridPos, Vector2 worldPos)
        {
            this.gridPos = gridPos;
            this.worldPos = worldPos;
        }

        public bool IsNeighboring(Node other)
        {
            return Mathf.Abs(other.gridPos.x - gridPos.x) <= 1 && Mathf.Abs(other.gridPos.y - gridPos.y) <= 1;
        }
        public bool IsPassible()
        {
            return passible && !blocked;
        }

        public void Init(GridNav nav)
        {
            passible = true;
            if (gridPos.x < nav.Borders / nav.UnitSize || gridPos.y < nav.Borders / nav.UnitSize  || 
                gridPos.x >= nav.width - nav.Borders / nav.UnitSize || gridPos.y >= nav.height - nav.Borders / nav.UnitSize)
            {
                passible = false;
            }
            else
            {
                foreach (RaycastHit2D collision in Physics2D.CircleCastAll(worldPos, nav.UnitSize, Vector2.zero))
                {
                    if (collision.transform.tag == "Obstacle")
                    {
                        passible = false;
                    }
                }
            }
            neighbors = new Node[]{
                nav.GetNodeAt(gridPos + Vector2Int.right + Vector2Int.up),
                nav.GetNodeAt(gridPos +  Vector2Int.up),
                nav.GetNodeAt(gridPos + Vector2Int.left + Vector2Int.up),
                nav.GetNodeAt(gridPos +  Vector2Int.left),
                nav.GetNodeAt(gridPos +  Vector2Int.right),
                nav.GetNodeAt(gridPos + Vector2Int.right + Vector2Int.down),
                nav.GetNodeAt(gridPos +  Vector2Int.down),
                nav.GetNodeAt(gridPos + Vector2Int.left + Vector2Int.down),
                };
        }
        public void SetBlocked(bool value)
        {
            blocked = value;
        }
    }
    private void Awake()
    {
        InitGrid();
    }
    int width;
    int height;
    Vector2 origin;
    void InitGrid()
    {
        width = Mathf.CeilToInt( transform.lossyScale.x / UnitSize);
        height = Mathf.CeilToInt(transform.lossyScale.y / UnitSize);
        origin = new Vector2(transform.position.x - transform.lossyScale.x * .5f, transform.position.y - transform.lossyScale.y * .5f);

        Nodes = new Node[width, height];
        for (int iX = 0; iX < width; iX++)
        {
            for (int iY = 0; iY < height; iY++)
            {
                Nodes[iX, iY] = new Node(new (iX,iY), new Vector2(origin.x + iX * UnitSize, origin.y + iY * UnitSize));
            }
        }
        foreach (Node n in Nodes)
        {
            n.Init(this);
        }
    }
    public int GetWidth()
    {
        return width;
    }
    public int GetHeight()
    {
        return height;
    }
    public bool GetNodeAt(Vector2 pos, out Node node)
    {
        node = GetNodeAt(TranslateCoordinate(pos));
        return node != null;
    }
    public Node GetNodeAt(Vector2Int pos)
    {
        if (pos.x < 0 || pos.y < 0 || pos.x >= Nodes.GetLength(0) || pos.y >= Nodes.GetLength(1))
            return null;
        return Nodes[pos.x, pos.y];
    }
    public Node GetNodeDirection(Vector2 center, Vector2 direction, float maxLength)
    {
        Vector2Int dir = Vector2Int.RoundToInt(direction);
        return GetNodeDirection(TranslateCoordinate(center), dir,Mathf.CeilToInt(maxLength / UnitSize));
    }
    public Node GetNodeDirection(Vector2Int center, Vector2Int direction, int maxLength)
    {
        Node oNode = GetNodeAt(center);
        for (int i = 0; i< maxLength; i++)
        {
            Node temp = GetNodeAt(center + direction * i);
            if (temp!=null && temp.IsPassible())
            {
                oNode = temp;
            }
            else
            {
                break;
            }    
        }
        return oNode;
    }
    void OnDrawGizmosSelected()
    {
        if (Nodes!=null)
        foreach (Node n in Nodes)
        {
            Gizmos.color = n.IsPassible() ? Color.green : Color.red;
            Gizmos.DrawSphere(n.worldPos, UnitSize * .5f);
        }
    }
    public Node GetClosestToPoint(Vector2Int point)
    {
        float sqrDist = Mathf.Infinity;
        Node dest = null;
        foreach (Node n in Nodes)
        {
            if (n.IsPassible())
            {
                float dist = (n.gridPos - point).sqrMagnitude;
                if (dist < sqrDist)
                {
                    dest = n;
                    sqrDist = dist;
                }
            }
        }
        return dest;
    }
    public Vector2Int TranslateCoordinate(Vector2 point)
    {
        return Vector2Int.CeilToInt((point - origin - Vector2.one * .5f * UnitSize) / UnitSize);
    }
    public Node[] GetNodesInCircle(Vector2 center, float radius)
    {
        return GetNodesInCircle(TranslateCoordinate(center), Mathf.RoundToInt(radius / UnitSize));
    }
    public Node[] GetNodesInCircle(Vector2Int center, int radius)
    {
        Vector2Int start = center - radius * Vector2Int.one;
        Vector2Int end = center + radius * Vector2Int.one;

        List<Node> nodes = new List<Node>();
        for (Vector2Int pos = start; true; pos.x++)
        {
            if ((pos - center).sqrMagnitude < radius * radius)
            nodes.Add(GetNodeAt(pos));
            if (pos.x > end.x)
            { pos.x = start.x; pos.y++; 
            if (pos.y > end.y)
            {
                break;
            }
            }
        }
        return nodes.ToArray();
    }
    public Node[] GetNodesInBox(Vector2 start, Vector2 end)
    {
        return GetNodesInBox(TranslateCoordinate(start),TranslateCoordinate(end));
    }
    public Node[] GetNodesInBox(Vector2Int start, Vector2Int end)
    {
        List<Node> nodes = new List<Node>();    
        for (Vector2Int pos = start; true; pos.x++)
        {
            nodes.Add(GetNodeAt(pos));
            if (pos.x > end.x)
            { pos.x = start.x; pos.y++; 
            if (pos.y > end.y)
            {
                break;
            }
            }
        }
        return nodes.ToArray();
    }
}
