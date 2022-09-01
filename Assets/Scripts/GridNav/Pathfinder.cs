using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;

public class Pathfinder
{
    public enum Failure
    {
        incomplete,
        impossible_outofrange,
        impassible_target,

        success
    }
    public int MinRange = 0;
    public int MaxRange = 1000;
    public static int MaxSolutions = 100;

    Vector2Int origin;
    Vector2Int dest;

    GridNav grid;
    GridNav.Node currentCell ;
    GridNav.Node originCell;
    GridNav.Node destinationCell;

    PathfinderPath currentPath;
    List<PathfinderPath> paths;

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

        public PathfinderPath(GridNav.Node startcell)
        {
            walkpath = new List<GridNav.Node>();
            walkpath.Add(startcell);
            FailureType = Failure.incomplete;
        }
        public PathfinderPath Clone(GridNav.Node extension)
        {
            PathfinderPath nPath = new PathfinderPath();
            nPath.walkpath.AddRange(walkpath);
            nPath.walkpath.Add(extension);
            return nPath;
        }
        public GridNav.Node Last()
        {
            return walkpath[walkpath.Count - 1];
        }


        public void SmoothPath()
        {
            Debug.Log("Smooth path with " + walkpath.Count + " tiles");
            bool iscompletelysmooth = false;
            while (!iscompletelysmooth)
            {
                iscompletelysmooth = true;

                for (int I = 0; I < walkpath.Count - 2; I++)
                {
                    GridNav.Node current = walkpath[I];
                    GridNav.Node next = walkpath[I + 2];

                    if (next.IsNeighboring(current))
                    {
                        walkpath.RemoveAt(I + 1);
                        iscompletelysmooth = false;
                    }
                }
            }
            Debug.Log("End result is " + walkpath.Count + " tiles");
        }

        public void Cull(int desiredLength)
        {
            if (desiredLength>0 && FailureType == Failure.success && walkpath.Count > desiredLength)
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
        public GridNav.Node Next()
        {
            position++;
            if (Solved())
            {
                return walkpath[walkpath.Count - 1];
            }
            return walkpath[position];
        }
        public void Undo()
        {
            position--;
        }
        public void Reset()
        {
            position = -1;
        }
        public bool Solved()
        {
            return position >= walkpath.Count - 1;
        }
    }

    List<GridNav.Node> searchTiles = new List<GridNav.Node>();

    public Pathfinder(GridNav g, Vector2Int origin, Vector2Int dest)
    {
        grid = g;
        this.origin = origin;
        this.dest = dest;
        paths = new List<PathfinderPath> { };
        MaxRange = grid.GetWidth() * grid.GetHeight();
    }

    public PathfinderPath GetMostImportantPath()
    {
        return paths[0];
    }

    public PathfinderPath GetFirstUnsolvedPath()
    {
        foreach (PathfinderPath nPath in paths)
        {
            if (nPath.FailureType == Failure.incomplete)
            {
                return nPath;
            }
        }
        return null;
    }
    public PathfinderPath GetShortestPath()
    {
        PathfinderPath path = paths[0];
        int len = path.walkpath.Count;
        foreach (PathfinderPath nPath in paths)
        {
            if (nPath.FailureType == Failure.success && nPath.walkpath.Count < len)
            {
                path =  nPath;
                len = nPath.walkpath.Count;
            }
        }
        return path;
    }

    Failure failure = Failure.incomplete;
    public void UpdateFailureType()
    {
        if (paths.Count > 0)
            failure = GetMostImportantPath().FailureType;
        else
            failure = Failure.incomplete;
    }
    public Failure GetFailureType()
    {
        return failure;
    }

    public List<GridNav.Node> GetWalkPath(bool include_origin)
    {
        List<GridNav.Node> value = GetMostImportantPath().walkpath;
        if (!include_origin)
        { value.RemoveAll((GridNav.Node T) => { return T.gridPos == origin; }); }
        return value;
    }

    public static PathfinderPath Solve(GridNav g, Vector2Int origin, Vector2Int dest, int aprRange)
    {
        Pathfinder pf = new Pathfinder(g, origin, dest);
        pf.MinRange = aprRange;
        pf.solve();
        return pf.GetMostImportantPath();
    }

    public static PathfinderPath SolvePath(GridNav g, Vector2 origin, Vector2 dest)
    {
        return Solve(g, g.TranslateCoordinate(origin), g.TranslateCoordinate(dest), 0);
    }
    public static PathfinderPath SolvePath(GridNav g, Vector2 origin, Vector2 dest,float range)
    {
        return Solve(g, g.TranslateCoordinate(origin), g.TranslateCoordinate(dest),Mathf.FloorToInt(range/g.UnitSize));
    }

    public void solve()
    {
        UnityEngine.Debug.Log("Initializing Pathfinder");
        originCell = grid.GetNodeAt(origin);
        paths = new List<PathfinderPath> { new PathfinderPath(originCell) };
        while (GetFirstUnsolvedPath() != null && GetFailureType() != Failure.success)
        {
            //find current path
            currentPath = GetFirstUnsolvedPath();
            currentCell = currentPath.Last();
            destinationCell = grid.GetNodeAt(dest);

            if (!grid.GetNodeAt(destinationCell.gridPos).passible && MinRange == 0)
            {
                UnityEngine.Debug.LogWarning("Target Impassible "+ MinRange);
                currentPath.FailureType = Failure.impassible_target;
            }

            if (destinationCell.gridPos == originCell.gridPos)
            {
                UnityEngine.Debug.LogWarning("Target Origin");
                return;
            }
            //solve paths
            int iter = 0;
            while (currentPath.FailureType == Failure.incomplete && iter < MaxRange)
            {
                iter++;
                stepPathfinder();
            }
            if (currentPath.FailureType == Failure.success)
            {
                currentPath.SmoothPath();
            }
            if (paths.Count>MaxSolutions)
            {
                PathfinderPath shortest = GetShortestPath();
                paths.RemoveAll((PathfinderPath item) => { return shortest.walkpath.Count > shortest.walkpath.Count; });
            }
            UpdateFailureType();
        }
        if (GetFailureType() < 0)
        {
            UnityEngine.Debug.LogError("Problem");
        }
        if (GetFailureType() == Failure.success)
        {
            Debug.Log("[Pathfinder] Success at point " + destinationCell.gridPos + " with " + paths.Count + " possible paths.");
           
            paths.RemoveAll((PathfinderPath item) => { return item.FailureType != Failure.success; });
            Debug.Log("[Pathfinder] Out of those, " + paths.Count + " reached the target.");


            paths.Sort((PathfinderPath A, PathfinderPath B) =>
            {
                    return A.walkpath.Count.CompareTo(B.walkpath.Count);
            });
        }
        /*else if (MinRange > 0)
        {
            Vector2Int nDest = dest;
            foreach (PathfinderPath path in paths)
            {
                Vector2Int conclude = path.Last().gridPos;
                if ((conclude - dest).sqrMagnitude <= MinRange)
                {
                    nDest = conclude;
                }
            }
            MinRange = 0;
            solve();
        }*/
        Debug.Log("Pathfinder concluded as " + GetFailureType() + " with npaths at " + paths.Count);
    }
    private void stepPathfinder()
    {
        searchTiles.Clear();

        if (!currentPath.walkpath.Contains(currentCell))
        {
            currentPath.walkpath.Add(currentCell);
        }
        //(approachRange>0 && (currentCell.gridPos - destinationCell.gridPos).sqrMagnitude <= approachRange* approachRange) || 
        if (currentCell == destinationCell)
        {
            currentPath.FailureType = Failure.success;
            return;
        }
        else if (MinRange > 0 && (currentCell.gridPos - dest).sqrMagnitude <= MinRange * MinRange)
        {
            currentPath.FailureType = Failure.success;
            return;
        }
        else if (MaxRange > 0 && currentPath.walkpath.Count > MaxRange )
        {
            Debug.LogWarning("Location Out Of Range: ({" + MaxRange + "," + currentPath.walkpath.Count + ")");
            currentPath.FailureType = Failure.impossible_outofrange;
            return;
        }
        List<GridNav.Node> adjacentCells = new List<GridNav.Node>();

        foreach (GridNav.Node Gir in grid.GetNodeAt(currentCell.gridPos).neighbors)
        {
            if (Gir!=null && Gir.passible && !currentPath.walkpath.Contains(Gir))
            {
                adjacentCells.Add(Gir);
            }
        }

        foreach (GridNav.Node n in adjacentCells)
        {
            float distance = GetDistanceBetweenTilesSquared(destinationCell, n);
            if (!searchTiles.Contains(n))
            {
                n.sqrDistance = distance;
                n.Previous = currentCell;
                searchTiles.Add(n);
            }
            else if (currentCell.Previous != null && n.sqrDistance < currentCell.Previous.sqrDistance)
            {
                currentCell.Previous = n;
                currentCell.sqrDistance = distance;
            }
        }

        if (searchTiles.Count == 0)
        {
            currentPath.FailureType = Failure.impassible_target;
        }
        else if (searchTiles.Contains(destinationCell))
        {
            currentCell = destinationCell;
        }
        else
        {
            if (searchTiles.Count > 1)
            {
                searchTiles.Sort((t1, t2) => t1.sqrDistance.CompareTo(t2.sqrDistance));

                if (searchTiles[0].gridPos.x != currentCell.gridPos.x && searchTiles[0].gridPos.y != currentCell.gridPos.y)
                {
                    paths.Add(currentPath.Clone(searchTiles[1]));
                }
            }
            currentCell = searchTiles[0];
        }
    }

    public static float GetDistanceBetweenTilesSquared(GridNav.Node A, GridNav.Node B)
    {
        return (A.gridPos - B.gridPos).sqrMagnitude;
    }
}