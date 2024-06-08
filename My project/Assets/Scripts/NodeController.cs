/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphNode
{
    public Vector2Int Position { get; private set; }
    public float GValue { get; set; }
    public float HValue { get; set; }
    public GraphNode CameFrom { get; set; }

    public GraphNode(int x, int y)
    {
        Position = new Vector2Int(x, y);
    }

    public float F()
    {
        return GValue + HValue;
    }
}

public class Graph
{
    private int width;
    private int height;
    private GraphNode[,] nodes;

    public Graph(int width, int height)
    {
        this.width = width;
        this.height = height;
        nodes = new GraphNode[width, height];
        
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                nodes[x, y] = new GraphNode(x, y);
            }
        }
    }

    public GraphNode GetNode(int x, int y)
    {
        if (x >= 0 && x < width && y >= 0 && y < height)
        {
            return nodes[x, y];
        }
        return null;
    }

    public List<GraphNode> GetNeighbors(GraphNode node)
    {
        List<GraphNode> neighbors = new List<GraphNode>();

        Vector2Int[] directions = {
            new Vector2Int(1, 0),
            new Vector2Int(-1, 0),
            new Vector2Int(0, 1),
            new Vector2Int(0, -1)
        };

        foreach (var dir in directions)
        {
            GraphNode neighbor = GetNode(node.Position.x + dir.x, node.Position.y + dir.y);
            if (neighbor != null)
            {
                neighbors.Add(neighbor);
            }
        }

        return neighbors;
    }
}

*/