using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Node
{
    public Vector2 position;
    public bool walkable;
    public float gScore;
    public float hScore;
    public float fScore;
    public Node parent;

    public Node(Vector2 position, bool walkable)
    {
        this.position = position;
        this.walkable = walkable;
    }
}