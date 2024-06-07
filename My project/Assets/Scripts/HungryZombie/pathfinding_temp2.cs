using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Graph
{
    private Node[,] nodes;

    public void CreateGraph(string[] mapLayout)
{
    int rows = mapLayout.Length;
    int cols = mapLayout[0].Length;

    nodes = new Node[rows, cols];

    Debug.Log(rows);
    Debug.Log(cols);
    for (int y = 0; y < cols; y++)
    {
        for (int x = 0; x < rows; x++)
        {
            bool isWall = mapLayout[x][y] == 1;
            Vector2 position = new Vector2(x, y);
            Node node = new Node(position, !isWall);
            nodes[x, y] = node;
        }
    }
}

    public List<Node> GetNeighbors(Node node)
{
    List<Node> neighbors = new List<Node>();

int[,] directions = new int[,]
{
    { 0, 1 },  // Up
    { 0, -1 }, // Down
    { -1, 0 }, // Left
    { 1, 0 }   // Right
};

for (int i = 0; i < directions.GetLength(0); i++)
{
    int newX = (int)node.position.x + directions[i, 0];
    int newY = (int)node.position.y + directions[i, 1];

    if (newX >= 0 && newX < nodes.GetLength(0) &&
        newY >= 0 && newY < nodes.GetLength(1) &&
        nodes[newX, newY].walkable)
    {
        neighbors.Add(nodes[newX, newY]);
    }
}
    return neighbors;
}

    public List<Node> FindPath(Vector2 startPos, Vector2 targetPos)
{
    Debug.Log(startPos);
    Debug.Log(nodes.Length);
    Node startNode = nodes[(int)startPos.y, (int)startPos.x];
    Node targetNode = nodes[(int)targetPos.y, (int)targetPos.x];

    List<Node> openSet = new List<Node>();
    HashSet<Node> closedSet = new HashSet<Node>();
    openSet.Add(startNode);

    while (openSet.Count > 0)
    {
        Node currentNode = openSet[0];
        for (int i = 1; i < openSet.Count; i++)
        {
            if (openSet[i].fScore < currentNode.fScore || 
                (openSet[i].fScore == currentNode.fScore && openSet[i].hScore < currentNode.hScore))
            {
                currentNode = openSet[i];
            }
        }

        openSet.Remove(currentNode);
        closedSet.Add(currentNode);

        if (currentNode == targetNode)
        {
            return RetracePath(startNode, targetNode);
        }

        foreach (Node neighbor in GetNeighbors(currentNode))
        {
            if (closedSet.Contains(neighbor))
            {
                continue;
            }

            float newCostToNeighbor = currentNode.gScore + GetDistance(currentNode, neighbor);
            if (newCostToNeighbor < neighbor.gScore || !openSet.Contains(neighbor))
            {
                neighbor.gScore = newCostToNeighbor;
                neighbor.hScore = GetDistance(neighbor, targetNode);
                neighbor.fScore = neighbor.gScore + neighbor.hScore;
                neighbor.parent = currentNode;

                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
            }
        }
    }
    return null; // No path found
}

private List<Node> RetracePath(Node startNode, Node endNode)
{
    List<Node> path = new List<Node>();
    Node currentNode = endNode;

    while (currentNode != startNode)
    {
        path.Add(currentNode);
        currentNode = currentNode.parent;
    }
    path.Add(startNode);
    path.Reverse();

    return path;
}

private float GetDistance(Node nodeA, Node nodeB)
{
    float dstX = Mathf.Abs(nodeA.position.x - nodeB.position.x);
    float dstY = Mathf.Abs(nodeA.position.y - nodeB.position.y);

    if (dstX > dstY)
        return 14 * dstY + 10 * (dstX - dstY);
    return 14 * dstX + 10 * (dstY - dstX);
}
}