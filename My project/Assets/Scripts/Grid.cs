using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Grid : MonoBehaviour
{
    //public LevelLoader LevelLoader;
    public Transform targetPosition;
    private Node[,] gridArray;
    public int gridSizex, gridSizey;
    public int cellSize;
    public LayerMask unWalkableLayer;

    private void Start() {
        CreateGrid();

    }

    private void CreateGrid()
    {
        gridArray = new Node[gridSizex, gridSizey];
        Vector2 worldBottomLeft = (Vector2)transform.position - Vector2.right * gridSizex / 2 - Vector2.up * gridSizey / 2;
        for(int x = 0; x < gridSizex; x++){
            for(int y = 0; y < gridSizey; y++){
                Vector2 worldPoint = worldBottomLeft + Vector2.right * (x * cellSize + cellSize / 2) + Vector2.up * (y * cellSize + cellSize / 2);
                bool isWalkable = !Physics2D.OverlapCircle(worldPoint, cellSize*0.1f, unWalkableLayer);

                gridArray[x,y] = new Node(isWalkable, worldPoint, x, y);
    
            }
        }
    }

    public List<Node> findPath(Vector2 startPosition, Vector2 targetPosition){
        Node startNode = GetNode(startPosition); 
        Node targetNode = GetNode(targetPosition);  

        if(!startNode.isWalkable || !targetNode.isWalkable){
            //no path
            return null;
        }
        List<Node> openSet = new List<Node>();
        HashSet<Node> closedSet = new HashSet<Node>();
        openSet.Add(startNode);
        while(openSet.Count > 0){
            Node currentNode = openSet[0];
            for(int i = 1; i < openSet.Count; i++){
                if(openSet[i].fvalue < currentNode.fvalue || openSet[i].fvalue == currentNode.fvalue && openSet[i].hvalue < currentNode.hvalue){
                    currentNode = openSet[i];
                }
            }
            openSet.Remove(currentNode);
            closedSet.Add(currentNode);

            if(currentNode == targetNode){
                return RetracePath(startNode, targetNode);
            }
            foreach(Node neighbour in GetNeighbours(currentNode)){
                if(!neighbour.isWalkable || closedSet.Contains(neighbour)){
                    continue;
                }
                int newMovementCostToNeighbour = currentNode.gvalue + GetDistance(currentNode, neighbour);
                if(newMovementCostToNeighbour < neighbour.gvalue ||!openSet.Contains(neighbour)){
                    neighbour.gvalue = newMovementCostToNeighbour;
                    neighbour.hvalue = GetDistance(neighbour, targetNode);
                    neighbour.parent = currentNode;

                    if(!openSet.Contains(neighbour)){
                        openSet.Add(neighbour);
                    }
                }
                
            }

        }
        //no path was found
        return null;
    }

    private List<Node> RetracePath(Node startNode, Node endNode){
        List<Node> path = new List<Node>();
        Node currentNode = endNode;

        while(currentNode!= startNode){
            path.Add(currentNode);
            currentNode = currentNode.parent;
        }
        path.Reverse();
        return path;
    }

    private int GetDistance(Node nodeA, Node nodeB){
        int distX = Mathf.Abs(nodeA.gridx - nodeB.gridx);
        int distY = Mathf.Abs(nodeA.gridy - nodeB.gridy);

        return distX + distY;
    }
    private List<Node> GetNeighbours(Node node){
        List<Node> neighbours = new List<Node>{};
        int[] xOffsets = { -1, 0, 1, 0};
        int[] yOffsets = { 0, 1, 0, -1};
        for(int i = 0; i < xOffsets.Length; i++){
            int x = node.gridx + xOffsets[i];
            int y = node.gridy + yOffsets[i];

            if(x >=0 && x < gridSizex && y >=0 && y < gridSizey){
                neighbours.Add(gridArray[x,y]);
            }
        }
        return neighbours;
    }

    public Node GetNode(Vector2 position){
        float closestNodeDist = Mathf.Infinity;
        Node nodePlaceHolder = new Node(true, new Vector2(0,0), 10, 10);

        foreach(Node node in gridArray){
            float dist = Vector2.Distance(position, node.worldPosition);
            if(dist < closestNodeDist){
                closestNodeDist = dist;
                nodePlaceHolder = node;
            }
        }
        return nodePlaceHolder;
    }
    private void OnDrawGizmos() {
        if(gridArray != null){
            foreach(Node node in gridArray){
                Gizmos.color = node.isWalkable ? Color.white : Color.red;
                Gizmos.DrawCube(node.worldPosition, Vector2.one * (cellSize - 0.4f));
    
            }
        }
    }
}
