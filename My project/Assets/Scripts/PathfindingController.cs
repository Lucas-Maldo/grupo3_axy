using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding
{
    public static List<GraphNode> FindPath(Graph graph, GraphNode start, GraphNode goal)
    {
        List<GraphNode> openSet = new List<GraphNode> { start };
        HashSet<GraphNode> closedSet = new HashSet<GraphNode>();

        start.GValue = 0;
        start.HValue = Vector2Int.Distance(start.Position, goal.Position);

        while (openSet.Count > 0)
        {
            GraphNode current = openSet[0];
            for (int i = 1; i < openSet.Count; i++)
            {
                if (openSet[i].F() < current.F() || (openSet[i].F() == current.F() && openSet[i].HValue < current.HValue))
                {
                    current = openSet[i];
                }
            }

            if (current == goal)
            {
                return ReconstructPath(current);
            }

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (GraphNode neighbor in graph.GetNeighbors(current))
            {
                if (closedSet.Contains(neighbor))
                {
                    continue;
                }

                float tentativeGValue = current.GValue + Vector2Int.Distance(current.Position, neighbor.Position);

                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
                else if (tentativeGValue >= neighbor.GValue)
                {
                    continue;
                }

                neighbor.CameFrom = current;
                neighbor.GValue = tentativeGValue;
                neighbor.HValue = Vector2Int.Distance(neighbor.Position, goal.Position);
            }
        }

        return null;
    }

    private static List<GraphNode> ReconstructPath(GraphNode current)
    {
        List<GraphNode> totalPath = new List<GraphNode> { current };

        while (current.CameFrom != null)
        {
            current = current.CameFrom;
            totalPath.Insert(0, current);
        }

        return totalPath;
    }
}
