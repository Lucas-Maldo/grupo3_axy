using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public bool isWalkable;
    public Vector2 worldPosition;

    public Node (bool isWalkable, Vector2 worldPosition){
        this.isWalkable = isWalkable;
        this.worldPosition = worldPosition;
    }
}
