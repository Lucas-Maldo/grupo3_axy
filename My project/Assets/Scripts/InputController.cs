using UnityEngine;
using UnityEditor;

public class InputController : MonoBehaviour
{
    public Vector2 Update()
    {
        float direction_x = 0;
        float direction_y = 0;

        if (Input.GetKey(KeyCode.W))
        {
            direction_y = 1f;
        }
        if (Input.GetKey(KeyCode.S))
        {
            direction_y = -1f;
        }
        if (Input.GetKey(KeyCode.A))
        {
            direction_x = -1f;
        }
        if (Input.GetKey(KeyCode.D))
        {
            direction_x = 1f;
        }

        return new Vector2(direction_x, direction_y);
    }
}