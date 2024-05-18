using UnityEngine;

public class StateController : MonoBehaviour
{
    public float startY = -3.0f; 
    public float endY = 3.0f;
    public bool moveUp = true;

    public bool CheckDirection(Vector2 position) 
    {
        if (position.y >= endY)
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