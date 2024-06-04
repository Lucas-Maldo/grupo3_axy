using UnityEngine;

public class PhysicsController : MonoBehaviour
{
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Movement(Vector2 direction, float velocity, GameObject player)
    {
        Vector2 movement = direction.normalized * velocity;
        rb.velocity = movement;
    }
}