using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HungryZombieController : MonoBehaviour
{
    private GameObject player;
    public float speed = 2f;
    private Graph graph;
    private List<GraphNode> path;
    private int pathIndex = 0;
    private float timer = 0;
    private float pathUpdateInterval = 2f; // Update path every second
    private int yOffset;
    private bool isFindingPath = false;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        int graphWidth = 62; 
        int graphHeight = 33; 
        yOffset = 17;
        graph = new Graph(graphWidth, graphHeight + yOffset);
        UpdatePath();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= pathUpdateInterval)
        {
            timer = 0;
            UpdatePath();
        }

        if (path != null && pathIndex < path.Count)
        {
            Vector2 targetPosition = path[pathIndex].Position;
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

            if ((Vector2)transform.position == targetPosition)
            {
                pathIndex++;
            }
        }
    }

    void UpdatePath()
    {
        Debug.Log((int)transform.position.x);
        Debug.Log((int)transform.position.y);
        Debug.Log((int)player.transform.position.x);
        Debug.Log((int)player.transform.position.y);
        GraphNode startNode = graph.GetNode((int)transform.position.x, (int)transform.position.y + yOffset);
        GraphNode goalNode = graph.GetNode((int)player.transform.position.x, (int)player.transform.position.y + yOffset);

        Debug.Log(graph);
        Debug.Log(startNode);
        Debug.Log(goalNode); 
        path = Pathfinding.FindPath(graph, startNode, goalNode);
        Debug.Log(path);
        pathIndex = 0;
    }
}

