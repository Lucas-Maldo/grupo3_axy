using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HungryZombie : MonoBehaviour
{
    private Grid grid;
    public List<Node> pathNodes = new List<Node>();
    public Vector2 targetNode;
    public GameObject playerPosition;
    public float moveSpeed;
    private int currentNodeIndex;
    // Start is called before the first frame update
    void Start()
    {
        grid = FindAnyObjectByType<Grid>();
        playerPosition = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        currentNodeIndex = 0;
        targetNode = (Vector2)playerPosition.transform.position;
        pathNodes = grid.findPath(transform.position, targetNode);
    
        if(pathNodes == null)return;

        if((Vector2)transform.position == grid.GetNode(targetNode).worldPosition){
            return;//we are on our target node
        }
        if(currentNodeIndex < pathNodes.Count-1){
            transform.position = Vector2.MoveTowards(transform.position, pathNodes[currentNodeIndex+1].worldPosition, moveSpeed * Time.deltaTime);

            if((Vector2)transform.position == pathNodes[currentNodeIndex+1].worldPosition){
                currentNodeIndex++;
            }
        }
    }
}
