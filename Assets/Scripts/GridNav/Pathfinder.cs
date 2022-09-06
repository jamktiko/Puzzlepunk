using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;
using System.Drawing;

public class Pathfinder
{
    public enum Failure
    {
        incomplete,
        impossible_outofrange,
        impassible_target,
        impossible,
        success
    }

    public class Node
    {
        public GridNav.Node point;
        public float Distance;
        public int index;

        public Node(GridNav.Node point, float distance, int index)
        {
            this.point = point;
            Distance = distance;
            this.index = index;
        }
    }


    public int ApproachRange = 0;
    public int MaxRange = 1000;

    Vector2Int origin;
    Vector2Int dest;

    GridNav grid;
    Node currentCell;
    
    bool approximate = true;
    bool recalculate = true;

    List<Node> openList = new List<Node>();
    List<Node> closedList = new List<Node>();

    public class PathfinderPath
    {
        public int position = 0;
        public List<GridNav.Node> walkpath;
        public Failure FailureType;

        public PathfinderPath()
        {
            walkpath = new List<GridNav.Node>();
            FailureType = Failure.incomplete;
        }

        public void Cull(int desiredLength)
        {
            if (desiredLength>0 && walkpath.Count > desiredLength)
            {
                FailureType = Failure.impossible_outofrange;

                while (walkpath.Count > desiredLength)
                {
                    walkpath.RemoveAt(walkpath.Count - 1);
                }
            }
        }
    

        public GridNav.Node Current()
        {
            return walkpath[position];
        }
        public GridNav.Node Last()
        {
            return walkpath[walkpath.Count - 1];
        }
        public GridNav.Node Next()
        {
            position++;
            if (Solved())
            {
                return walkpath[walkpath.Count - 1];
            }
            return walkpath[position];
        }
        public bool Solved()
        {
            return position >= walkpath.Count - 1;
        }
    }

    public Pathfinder(GridNav g, Vector2Int origin, Vector2Int dest)
    {
        grid = g;
        this.origin = origin;
        this.dest = dest;
        MaxRange = grid.GetWidth() * grid.GetHeight();
    }
    public Failure GetFailureType()
    {
        return failure;
    }

    public List<GridNav.Node> GetWalkPath(bool include_origin)
    {
        List<GridNav.Node> value = GetResult().walkpath;
        if (!include_origin)
        { value.RemoveAll((GridNav.Node T) => { return T.gridPos == origin; }); }
        return value;
    }
    public PathfinderPath GetResult()
    {
        return finalPath;
    }

    public static PathfinderPath Solve(GridNav g, Vector2Int origin, Vector2Int dest, int aprRange)
    {
        Pathfinder pf = new Pathfinder(g, origin, dest);
        pf.ApproachRange = aprRange;
        pf.solve();
        return pf.GetResult();
    }

    public static PathfinderPath SolvePath(GridNav g, Vector2 origin, Vector2 dest)
    {
        return Solve(g, g.TranslateCoordinate(origin), g.TranslateCoordinate(dest), 0);
    }
    public static PathfinderPath SolvePath(GridNav g, Vector2 origin, Vector2 dest,float range)
    {
        return Solve(g, g.TranslateCoordinate(origin), g.TranslateCoordinate(dest),Mathf.Max(1, Mathf.FloorToInt(range/g.UnitSize - 1)));
    }

    Failure failure = Failure.incomplete;
    public void solve()
    {
        UnityEngine.Debug.Log("Initializing Pathfinder");
        currentCell = new Node(grid.GetNodeAt(origin), GetDistanceBetweenTilesSquared(origin, dest), 0);
        openList.Add(currentCell);

        if (dest == origin)
        {
            UnityEngine.Debug.LogWarning("Target Origin");
            return;
        }
        if (grid.GetNodeAt(dest) == null || !grid.GetNodeAt(dest).passible && ApproachRange == 0)
        {
            UnityEngine.Debug.LogWarning("Target Impassible " + ApproachRange);

            if (approximate)
            {
                dest = grid.GetClosestToPoint(dest).gridPos;
            }
            else
            {
                failure = Failure.impassible_target;
            }
        }

        while (failure == Failure.incomplete)
        {
            stepPathfinder();
        }

        if (failure != Failure.success && recalculate)
        {
            recalculate = false;

            float sqrDist = Mathf.Infinity;
            Node newDest = null;
            foreach (Node n in closedList)
            {
                float dist = (n.point.gridPos - dest).sqrMagnitude;
                if (n.point.passible && dist < sqrDist)
                {
                    newDest = n;
                    sqrDist = dist;
                }
            }
            openList.Clear();
            closedList.Clear();

            dest = newDest.point.gridPos;
            solve();
        }
        else
        {
            ResolvePath();
            Debug.Log("Pathfinder concluded as " + GetFailureType());
        }
    }
    private void stepPathfinder()
    {
        if (currentCell.point.gridPos == dest || (ApproachRange > 0 && (currentCell.point.gridPos - dest).sqrMagnitude <= ApproachRange * ApproachRange))
        {
            failure = Failure.success;
            return;
        }

        foreach (GridNav.Node n in currentCell.point.neighbors)
        {
            if (n != null && n.passible )
            {
                bool exists = false;
                foreach (var Zim in openList)
                {
                    if (Zim.point == n)
                    {
                        exists = true ;
                        break;
                    }
                }
                if (!exists)
                {
                    foreach (var Zim in closedList)
                    {
                        if (Zim.point == n)
                        {
                            exists = true;
                            break;
                        }
                    }
                }
                if (!exists)
                {
                    openList.Add(new(n, GetDistanceBetweenTilesSquared(dest, n.gridPos), currentCell.index + 1));
                }
            }
        }

        if (openList.Count == 0)
        {
            failure = Failure.impossible;
        }
        else
        {
            closedList.Add(currentCell);
            currentCell = openList[0];
            openList.RemoveAt(0);
            openList.Sort((Node A, Node B) => { return A.Distance.CompareTo(B.Distance); });
        }
    }
    PathfinderPath finalPath = new PathfinderPath();
    void ResolvePath()
    {
        finalPath.FailureType = GetFailureType();
        if (finalPath.FailureType == Failure.success)
        {
            List<Node> walkPath = new List<Node>();
            walkPath.Add(currentCell);

            while (currentCell.index != 0)
            {
                foreach (GridNav.Node neighbor in currentCell.point.neighbors)
                {
                    if (neighbor!=null && neighbor.passible)
                    {
                        foreach (Node Gir in closedList)
                        {
                            if (Gir.point == neighbor && Gir.index < currentCell.index)
                            {
                                currentCell = Gir;
                            }
                        }
                    }
                }
                walkPath.Add(currentCell);
            }

            walkPath.Reverse();
            foreach (Node node in walkPath)
            {
                finalPath.walkpath.Add(node.point);
            }

                //finalPath.Cull(MaxRange);
        }
    }

    public static float GetDistanceBetweenTilesSquared(GridNav.Node A, GridNav.Node B)
    {
        return GetDistanceBetweenTilesSquared(A.gridPos, B.gridPos);
    }

    public static float GetDistanceBetweenTilesSquared(Vector2 A, Vector2 B)
    {
        return (A - B).sqrMagnitude;
    }
}