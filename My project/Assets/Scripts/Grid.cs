using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Grid : MonoBehaviour
{
    //public LevelLoader LevelLoader;
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

                gridArray[x,y] = new Node(isWalkable, worldPoint);
    
            }
        }
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
