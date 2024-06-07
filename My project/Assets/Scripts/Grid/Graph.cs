using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class Graph
{
    private GraphNode[,] nodes;
    private int width;
    private int height;

    public Graph(List<List<char>> optimizedGrid)
    {
        height = optimizedGrid.Count;
        width = optimizedGrid[0].Count;
        nodes = new GraphNode[height, width];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                bool isWalkable = optimizedGrid[y][x] == '0';
                nodes[y, x] = new GraphNode(x, y, isWalkable);
            }
        }
    }

    public GraphNode GetNode(int x, int y)
    {
        return nodes[y, x];
    }

    public List<GraphNode> GetNeighbors(GraphNode node)
    {
        List<GraphNode> neighbors = new List<GraphNode>();

        for (int dx = -1; dx <= 1; dx++)
        {
            for (int dy = -1; dy <= 1; dy++)
            {
                if (dx == 0 && dy == 0) continue;

                int neighborX = node.X + dx;
                int neighborY = node.Y + dy;

                if (neighborX >= 0 && neighborX < width && neighborY >= 0 && neighborY < height)
                {
                    GraphNode neighbor = nodes[neighborY, neighborX];
                    if (neighbor.IsWalkable)
                    {
                        neighbors.Add(neighbor);
                    }
                }
            }
        }

        return neighbors;
    }

    public List<GraphNode> FindShortestPath(GraphNode start, GraphNode target)
{
    // Inicializar listas de nodos abiertos y cerrados
    List<GraphNode> openSet = new List<GraphNode>();
    HashSet<GraphNode> closedSet = new HashSet<GraphNode>();

    // Agregar el nodo de inicio a la lista abierta
    openSet.Add(start);

    // Inicializar valores de costo y camino
    foreach (GraphNode node in nodes)
    {
        node.GValue = float.PositiveInfinity;
        node.CameFrom = null;
    }
    start.GValue = 0;

    // Mientras haya nodos en la lista abierta
    while (openSet.Count > 0)
    {
        // Obtener el nodo con el costo F más bajo
        GraphNode currentNode = openSet.OrderBy(n => n.F()).First();

        // Si es el nodo destino, reconstruir y devolver el camino
        if (currentNode == target)
        {
            return ReconstructPath(start, target);
        }

        // Mover el nodo actual de la lista abierta a la cerrada
        openSet.Remove(currentNode);
        closedSet.Add(currentNode);

        // Evaluar los vecinos del nodo actual
        foreach (GraphNode neighbor in GetNeighbors(currentNode))
        {
            // Si el vecino está en la lista cerrada, ignorarlo
            if (closedSet.Contains(neighbor)) continue;

            float tentativeGValue = currentNode.GValue + CalculateDistance(currentNode, neighbor);

            // Si el vecino no está en la lista abierta o si su costo tentativo es menor que el actual
            if (!openSet.Contains(neighbor) || tentativeGValue < neighbor.GValue)
            {
                // Actualizar los valores del vecino
                neighbor.CameFrom = currentNode;
                neighbor.GValue = tentativeGValue;
                neighbor.HValue = CalculateDistance(neighbor, target);

                // Si el vecino no está en la lista abierta, agregarlo
                if (!openSet.Contains(neighbor))
                {
                    openSet.Add(neighbor);
                }
            }
        }
    }

    // Si no se encontró un camino, devolver una lista vacía
    return new List<GraphNode>();
}

private List<GraphNode> ReconstructPath(GraphNode start, GraphNode target)
{
    List<GraphNode> path = new List<GraphNode>();
    GraphNode current = target;

    while (current != start)
    {
        path.Insert(0, current);
        current = current.CameFrom;
    }

    path.Insert(0, start);
    return path;
}

private float CalculateDistance(GraphNode a, GraphNode b)
{
    return Mathf.Sqrt((a.X - b.X) * (a.X - b.X) + (a.Y - b.Y) * (a.Y - b.Y));
}
}