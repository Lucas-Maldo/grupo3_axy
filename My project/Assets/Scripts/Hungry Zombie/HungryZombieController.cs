using System.Linq;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class HungryZombieController : MonoBehaviour
{
    public Graph graph;
    public GameObject player;
    public List<GraphNode> path;
    private Vector3 lastPlayerPosition;
    private float updateInterval = 2.0f;

    void Awake()
    {
        int graphWidth = 62;
        int graphHeight = 33;
        Vector3 graphOffset = new Vector3(0, 0, 0);  // Assuming your world coordinates start from (-10, -10)
        graph = new Graph(graphWidth, graphHeight, graphOffset);
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.Log("Player is not set.");
            return;
        }

        lastPlayerPosition = player.transform.position;
        Debug.Log("Player last position: " + lastPlayerPosition);


        if (graph == null)
        {
            Debug.Log("Graph is not set.");
            return;
        }

        if (player == null)
        {
            Debug.Log("Player is not set.");
            return;
        }

        Vector3 enemyPosition = transform.position;
        Vector3 playerPosition = player.transform.position;

        Debug.Log("Enemy position: " + enemyPosition);
        Debug.Log("Player position: " + playerPosition);
        foreach (var node in graph.nodes)
        {
            Debug.Log("Node position: " + node.position);
        }

        if (!graph.IsPositionWithinBounds(enemyPosition) || !graph.IsPositionWithinBounds(playerPosition))
        {
            Debug.LogError("Enemy or player position is outside the bounds of the graph.");
            return;
        }

        Debug.Log("Graph and Player are set.");
        path = graph.AStarSearch(graph.GetNode(enemyPosition), graph.GetNode(playerPosition));
        if (path != null)
        {
            Debug.Log("Path found with " + path.Count + " nodes.");
        }
        else
        {
            Debug.Log("No path found.");
        }

        Debug.Log("-----------------------------------------------------------");
        Debug.Log("player graph: " + graph.GetNode(playerPosition));
        Debug.Log("enemy graph: " + graph.GetNode(enemyPosition));
        foreach (var node in graph.nodes)
        {
            Debug.Log("Node position: " + node.position);
        }

        //StartCoroutine(RecalculatePath());
    }

    private IEnumerator RecalculatePath()
    {
        while (true)
        {
            // Check if the player's position has changed
            if (player.transform.position != lastPlayerPosition)
            {
                // Update the last known player position
                lastPlayerPosition = player.transform.position;

                // Recalculate the path
                Vector3 enemyPosition = transform.position;
                Vector3 playerPosition = player.transform.position;

                GraphNode startNode = graph.GetNode(enemyPosition);
                GraphNode endNode = graph.GetNode(playerPosition);

                path = graph.AStarSearch(startNode, endNode);
            }

            yield return new WaitForSeconds(1f); 
        }
    }

    IEnumerator UpdatePath()
    {
        while (true)
        {
            Vector3 playerPosition = player.transform.position;
            if (playerPosition != lastPlayerPosition)
            {
                Debug.Log("Player moved to position " + playerPosition);
                path = graph.AStarSearch(graph.GetNode(transform.position), graph.GetNode(playerPosition));
                if (path != null)
                {
                    Debug.Log("New path found with " + path.Count + " nodes.");
                    if (path.Count > 0)
                    {
                        Debug.Log("First node in new path is at position " + path[0].position);
                    }
                }
                else
                {
                    Debug.Log("No path found to player's new position.");
                }
                lastPlayerPosition = playerPosition;
            }
            yield return new WaitForSeconds(updateInterval);
        }
    }

    void Update()
    {
        // Check if the player's position has changed
        if (player.transform.position != lastPlayerPosition)
        {
            // Update the last known player position
            lastPlayerPosition = player.transform.position;
    
            // Recalculate the path
            Vector3 enemyPosition = transform.position;
            Vector3 playerPosition = player.transform.position;
    
            GraphNode startNode = graph.GetNode(enemyPosition);
            GraphNode endNode = graph.GetNode(playerPosition);
    
            path = graph.AStarSearch(startNode, endNode);
        }
    
        if (path != null && path.Count > 0)
        {
            Vector3 targetPosition = path[0].position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime);
    
            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                Debug.Log("Reached node at position " + targetPosition + ". Zombie is actually at position " + transform.position);
                path.RemoveAt(0);
            }
            else
            {
                Debug.Log("Moving towards node at position " + targetPosition + ". Player is at position " + player.transform.position);
            }
        }
    }
}

public class Graph
{
    public GraphNode[,] nodes;
    private Vector3 offset;

    public Graph(int width, int height, Vector3 offset)
    {
        nodes = new GraphNode[width, height];
        this.offset = offset;

        // Initialize the nodes
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                nodes[x, y] = new GraphNode(new Vector3(x + offset.x, y + offset.y, 0));
            }
        }
    }

    public GraphNode GetNode(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x - offset.x);
        int y = Mathf.FloorToInt(position.y - offset.y);

        Debug.Log("Transforming world position " + position + " to graph position: (" + x + ", " + y + ")");

        if (x < 0 || x >= nodes.GetLength(0) || y < 0 || y >= nodes.GetLength(1))
        {
            Debug.LogError("Position is outside the bounds of the graph: " + position + ", converted: " + new Vector3(x, y, 0));
            return null;
        }

        Debug.Log("Position is inside the bounds of the graph: " + x + ", " + y);

        return nodes[x, y];
    }

    public List<GraphNode> AStarSearch(GraphNode startNode, GraphNode endNode)
    {
        if (startNode == null || endNode == null)
        {
            Debug.LogError("Start or end node is null.");
            return null;
        }

        HashSet<GraphNode> openList = new HashSet<GraphNode>();
        HashSet<GraphNode> closedList = new HashSet<GraphNode>();

        openList.Add(startNode);

        while (openList.Count > 0)
        {
            GraphNode currentNode = openList.OrderBy(node => node.F()).First();

            if (currentNode == endNode)
            {
                List<GraphNode> path = new List<GraphNode>();
                while (currentNode != null)
                {
                    path.Add(currentNode);
                    currentNode = currentNode.CameFrom;
                }
                path.Reverse();
                return path;
            }

            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (GraphNode neighborNode in GetNeighbors(currentNode))
            {
                if (closedList.Contains(neighborNode)) continue;

                float tentativeGValue = currentNode.GValue + Vector3.Distance(currentNode.position, neighborNode.position);

                if (openList.Contains(neighborNode) && tentativeGValue >= neighborNode.GValue) continue;

                neighborNode.CameFrom = currentNode;
                neighborNode.GValue = tentativeGValue;
                neighborNode.HValue = Vector3.Distance(neighborNode.position, endNode.position);

                if (!openList.Contains(neighborNode)) openList.Add(neighborNode);
            }
        }

        return null;  // No path found
    }

    public List<GraphNode> GetNeighbors(GraphNode node)
    {
        List<GraphNode> neighbors = new List<GraphNode>();

        int x = Mathf.FloorToInt(node.position.x - offset.x);
        int y = Mathf.FloorToInt(node.position.y - offset.y);

        // Check the node above
        if (y + 1 < nodes.GetLength(1))
        {
            neighbors.Add(nodes[x, y + 1]);
        }

        // Check the node below
        if (y - 1 >= 0)
        {
            neighbors.Add(nodes[x, y - 1]);
        }

        // Check the node to the right
        if (x + 1 < nodes.GetLength(0))
        {
            neighbors.Add(nodes[x + 1, y]);
        }

        // Check the node to the left
        if (x - 1 >= 0)
        {
            neighbors.Add(nodes[x - 1, y]);
        }

        return neighbors;
    }

    public bool IsPositionWithinBounds(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x - offset.x);
        int y = Mathf.FloorToInt(position.y - offset.y);

        bool withinBounds = x >= 0 && x < nodes.GetLength(0) && y >= 0 && y < nodes.GetLength(1);
        Debug.Log("Checking if position " + position + " is within bounds: " + withinBounds);
        return withinBounds;
    }
}

public class GraphNode
{
    public Vector3 position;
    public GraphNode CameFrom;  // The node it came from
    public float GValue;  // Cost from start to this node
    public float HValue;  // Estimated cost from this node to end

    public GraphNode(Vector3 position)
    {
        this.position = position;
    }

    public float F()  // Total cost
    {
        return GValue + HValue;
    }
}
