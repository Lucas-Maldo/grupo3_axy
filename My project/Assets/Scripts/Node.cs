using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public int gvalue, hvalue;
    public int fvalue => gvalue + hvalue;
    public int gridx, gridy;
    public bool isWalkable;
    public Vector2 worldPosition;
    public Node parent;

    public Node (bool isWalkable, Vector2 worldPosition, int gridx, int gridy){
        this.isWalkable = isWalkable;
        this.worldPosition = worldPosition;
        this.gridx = gridx;
        this.gridy = gridy;
    }
}
