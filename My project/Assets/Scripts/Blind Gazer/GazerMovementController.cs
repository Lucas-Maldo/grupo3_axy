using UnityEngine;

public class MovementController : MonoBehaviour
{
    public float speed = 1.0f;
    public float startX = 2.0f;
    
    public void MoveEnemy(bool moveUp)
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
}