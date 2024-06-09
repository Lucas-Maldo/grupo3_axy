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
    private const float updateInterval = 1.0f; 

    void Awake()
    {
        graph = new Graph();
    }

    void Start()
    {
        FindPlayer();

        if (player == null)
        {
            Debug.Log("Player is not set.");
            return;
        }

        lastPlayerPosition = player.transform.position;
       
        if (graph == null)
        {
            Debug.Log("Graph is not set.");
            return;
        }

        
        Vector3 enemyPosition = transform.position;
        Vector3 playerPosition = player.transform.position;


        if (!IsPositionsWithinGraphBounds(enemyPosition, playerPosition))
        {
            Debug.LogError("Enemy or player position is outside the bounds of the graph.");
            return;
        }

        path = CalculatePathToPlayer(enemyPosition, playerPosition);
        if (path != null)
        {
            Debug.Log("Path found with " + path.Count + " nodes.");
        }
        else
        {
            Debug.Log("No path found.");
        }

        StartCoroutine(UpdatePath());
    }

    private void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    
    private bool IsPositionsWithinGraphBounds(Vector3 enemyPosition, Vector3 playerPosition)
    {
        return graph.IsPositionWithinBounds(enemyPosition) && graph.IsPositionWithinBounds(playerPosition);
    }

    private List<GraphNode> CalculatePathToPlayer(Vector3 enemyPosition, Vector3 playerPosition)
    {
        return graph.AStarSearch(graph.GetNode(enemyPosition), graph.GetNode(playerPosition));
    }


    private IEnumerator UpdatePath()
    {
        while (true)
        {
            Vector3 playerPosition = player.transform.position;

            if (playerPosition != lastPlayerPosition)
            {
                lastPlayerPosition = playerPosition;

                graph = new Graph();

                Vector3 enemyPosition = transform.position;

                if (!IsPositionsWithinGraphBounds(enemyPosition, playerPosition))
                {
                    Debug.LogError("Enemy or player position is outside the bounds of the graph.");
                    //break; // Aca hay que agregar el tema de si choca al jugador
                }

                path = CalculatePathToPlayer(enemyPosition, playerPosition);
                if (path != null)
                {
                    Debug.Log("Path found with " + path.Count + " nodes.");
                }
                else
                {
                    Debug.Log("No path found.");
                }
            }

            yield return new WaitForSeconds(updateInterval); 
        }
    }

    void Update()
    {
        if (path != null && path.Count > 0)
        {
            GraphNode nextNode = path[0];

            float sqrDistanceToTarget = (nextNode.position - transform.position).sqrMagnitude;

            if (sqrDistanceToTarget < 0.1f * 0.1f)
            {
                path.RemoveAt(0);
            }
            else
            {
                transform.position = Vector3.MoveTowards(transform.position, nextNode.position, Time.deltaTime);
            }
        }
    }

}

public class Graph
{
    public GraphNode[,] nodes;
    private Vector3 offset;
    private int graphWidth = 62;
    private int graphHeight = 33;
    Vector3 graphOffset = Vector3.zero;

    public Graph()
    {
        nodes = new GraphNode[graphWidth, graphHeight];
        this.offset = offset;

        Vector3Int offsetInt = new Vector3Int((int)offset.x, (int)offset.y, 0);

        for (int x = 0; x < graphWidth; x++)
        {
            for (int y = 0; y < graphHeight; y++)
            {
                Vector3Int nodePosition = new Vector3Int(x, y, 0) + offsetInt;
                nodes[x, y] = new GraphNode(nodePosition);
            }
        }
    }

    public GraphNode GetNode(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x - offset.x);
        int y = Mathf.FloorToInt(position.y - offset.y);

        if (x < 0 || x >= graphWidth || y < 0 || y >= graphHeight)
        {
            return null;
        }

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

        return null; 
    }

    public IEnumerable<GraphNode> GetNeighbors(GraphNode node)
    {
        int x = Mathf.FloorToInt(node.position.x - offset.x);
        int y = Mathf.FloorToInt(node.position.y - offset.y);

        int nodesLength0 = nodes.GetLength(0);
        int nodesLength1 = nodes.GetLength(1);

        if (y + 1 < nodesLength1) 
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
        if (x + 1 < nodesLength0) 
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
        Collider2D hitCollider = Physics2D.OverlapCircle(position, 0.2f);
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
        return withinBounds;
    }
}

public class GraphNode
{
    public Vector3 position;
    public GraphNode CameFrom;  
    public float GValue;  
    public float HValue;  
    public bool IsWall { get; set; }

    public GraphNode(Vector3 position)
    {
        this.position = position;
    }

    public float F() 
    {
        return GValue + HValue;
    }
}