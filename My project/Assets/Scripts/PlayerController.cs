using UnityEngine;
using UnityEditor;

public class PlayerController : MonoBehaviour
{
    public GameObject player;
    public float velocity;

    private Vector2 position;

    void Start()
    {
        InitializePlayer();
    }

    void Update()
    {
        Vector2 direction = ProcessInput();
        Movement(direction);
    }

    void InitializePlayer()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    Vector2 ProcessInput()
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

    void Movement(Vector2 direction)
    {
        position = direction.normalized;
        player.transform.Translate(Time.deltaTime * velocity * position);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision);
    }

    void HandleCollision(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "BlindGazer" || collision.gameObject.tag == "Skeleton")
        {
            Debug.Log("Collided with: " + collision.gameObject.tag);
            EditorApplication.Beep();
        }
    }
}