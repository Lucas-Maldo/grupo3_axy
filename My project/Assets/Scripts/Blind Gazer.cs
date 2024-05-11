using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindGazer : MonoBehaviour
{
    // Positions
    private float startX = 2.0f;
    private float startY = -3.0f;
    private float endY = 3.0f;

    // Movement
    public bool moveUp = true; 
    public float speed = 1.0f;

    void Update()
    {
        UpdateState();
    }

    void UpdateState() 
    {
        MoveEnemy();
        CheckDirection(transform.position);
    }

    void MoveEnemy()
    {
        if (moveUp)
        {
            MoveUp();
        }
        else
        {
            MoveDown();
        }
    }

    void MoveUp()
    {
        transform.position = new Vector2(startX, transform.position.y + speed * Time.deltaTime);
    }

    void MoveDown()
    {
        transform.position = new Vector2(startX, transform.position.y - speed * Time.deltaTime);
    }

    void CheckDirection(Vector2 position) 
    {
        if (position.y >= endY)
        {
            moveUp = false;
        }
        else if (position.y <= startY)
        {
            moveUp = true;
        }
    }
}