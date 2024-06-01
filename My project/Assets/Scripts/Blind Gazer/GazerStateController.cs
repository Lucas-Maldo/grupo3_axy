using UnityEngine;

public class StateController : MonoBehaviour
{
    public float startY; 
    public float endY = 5.0f;
    public bool moveUp = true;

    void Start()
    {
        startY = transform.position.y;
    }

    public bool CheckDirection(Vector2 position) 
    {
        Debug.Log("Position Y - Start Y: " + (position.y - startY));
        
        if (position.y - startY >= endY)
        {
            moveUp = false;
        }
        else if (position.y <= startY)
        {
            moveUp = true;
        }
        return moveUp;
    }
}