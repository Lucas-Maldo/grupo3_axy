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
    private const float updateInterval = 1.0f;  // Update interval set to 1 second

    void Awake()
    {
        int graphWidth = 62;
        int graphHeight = 33;
        Vector3 graphOffset = Vector3.zero;  // Assuming your world coordinates start from (0, 0)
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

        Vector3 enemyPosition = transform.position;
        Vector3 playerPosition = player.transform.position;

        Debug.Log("Enemy position: " + enemyPosition);
        Debug.Log("Player position: " + playerPosition);

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

        //StartCoroutine(UpdatePath());
    }

    private IEnumerator UpdatePath()
    {
        while (true)
        {
            Vector3 playerPosition = player.transform.position;

            if (playerPosition != lastPlayerPosition)
            {
                lastPlayerPosition = playerPosition;

                if (!graph.IsPositionWithinBounds(transform.position) || !graph.IsPositionWithinBounds(playerPosition))
                {
                    Debug.LogError("Enemy or player position is outside the bounds of the graph.");
                }
                else
                {
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
                }
            }

            yield return new WaitForSeconds(1f); // Wait for 1 second
        }
    }

    void Update()
    {
        if (path != null && path.Count > 0)
        {
            Vector3 targetPosition = path[0].position;
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime);

            if (Vector3.Distance(transform.position, targetPosition) < 0.1f)
            {
                path.RemoveAt(0);
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

        var openList = new HashSet<GraphNode> { startNode };
        var closedList = new HashSet<GraphNode>();

        while (openList.Count > 0)
        {
            var currentNode = openList.OrderBy(node => node.F()).First();

            if (currentNode == endNode)
            {
                var path = new List<GraphNode>();
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

            foreach (var neighborNode in GetNeighbors(currentNode))
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

    public IEnumerable<GraphNode> GetNeighbors(GraphNode node)
    {
        int x = Mathf.FloorToInt(node.position.x - offset.x);
        int y = Mathf.FloorToInt(node.position.y - offset.y);
    
        if (y + 1 < nodes.GetLength(1)) 
        {
            var neighbor = nodes[x, y + 1];
            neighbor.IsWall = IsPositionBlocked(neighbor.position);
            if (!neighbor.IsWall) yield return neighbor;
        }
        if (y - 1 >= 0) 
        {
            var neighbor = nodes[x, y - 1];
            neighbor.IsWall = IsPositionBlocked(neighbor.position);
            if (!neighbor.IsWall) yield return neighbor;
        }
        if (x + 1 < nodes.GetLength(0)) 
        {
            var neighbor = nodes[x + 1, y];
            neighbor.IsWall = IsPositionBlocked(neighbor.position);
            if (!neighbor.IsWall) yield return neighbor;
        }
        if (x - 1 >= 0) 
        {
            var neighbor = nodes[x - 1, y];
            neighbor.IsWall = IsPositionBlocked(neighbor.position);
            if (!neighbor.IsWall) yield return neighbor;
        }
    }

    public bool IsPositionBlocked(Vector3 position)
    {

        Collider2D hitCollider = Physics2D.OverlapCircle(position, 0.5f);
        Debug.Log(position);
        if (hitCollider != null && hitCollider.gameObject.tag == "Wall")
        {
            return true;
        }
    
        return false;
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
    public bool IsWall { get; set; }

    public GraphNode(Vector3 position)
    {
        this.position = position;
    }

    public float F()  // Total cost
    {
        return GValue + HValue;
    }
}