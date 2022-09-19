using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridNav : MonoBehaviour
{
    public static GridNav main;
    public float UnitSize = 1;

    Node[,] Nodes;

    public class Node
    {
        public Vector2Int gridPos;
        public Vector2 worldPos;
        public bool passible;
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

        public void Init(GridNav nav)
        {
            passible = true;
            foreach (RaycastHit2D collision in Physics2D.CircleCastAll(worldPos, nav.UnitSize, Vector2.zero) )
            {
                if (collision.transform.tag == "Obstacle")
                {
                    passible = false;
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
        public override bool Equals(object obj)
        {
            if ((obj == null) || !this.GetType().Equals(obj.GetType()))
            {
                return false;
            }
            else
            {
                Node p = (Node)obj;
                return gridPos == p.gridPos;
            }
        }
    }
    private void Awake()
    {
        main = this;
    }
    private void Start()
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
    public Node GetNodeAt(Vector2Int pos)
    {
        if (pos.x < 0 || pos.y < 0 || pos.x >= Nodes.GetLength(0) || pos.y >= Nodes.GetLength(1))
            return null;
        return Nodes[pos.x, pos.y];
    }
    public Node GetNodeDirection(Vector2 center, Vector2 direction, float maxLength)
    {
        Vector2Int dir = Vector2Int.zero;
        if (direction.y > 0)
            dir = Vector2Int.up;
        else if (direction.y < 0)
            dir = Vector2Int.down;
        else if (direction.x > 0)
            dir = Vector2Int.right;
        else if (direction.x < 0)
            dir = Vector2Int.left;

        return GetNodeDirection(TranslateCoordinate(center), dir,Mathf.CeilToInt(maxLength / UnitSize));
    }
    public Node GetNodeDirection(Vector2Int center, Vector2Int direction, int maxLength)
    {
        Node oNode = GetNodeAt(center);
        for (int i = 0; i< maxLength; i++)
        {
            Node temp = GetNodeAt(center + direction * maxLength);
            if (temp!=null && temp.passible)
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
    void OnDrawGizmos()
    {
        if (Nodes!=null)
        foreach (Node n in Nodes)
        {
            Gizmos.color = n.passible ? Color.green : Color.red;
            Gizmos.DrawSphere(n.worldPos, .1f);
        }
    }
    public Node GetClosestToPoint(Vector2Int point)
    {
        float sqrDist = Mathf.Infinity;
        Node dest = null;
        foreach (Node n in Nodes)
        {
            if (n.passible)
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
}
