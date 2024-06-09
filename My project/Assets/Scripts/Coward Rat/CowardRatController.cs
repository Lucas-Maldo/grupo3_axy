using UnityEngine;

public class CowardRatController : MonoBehaviour
{
    public float wanderSpeed = 2.0f;
    public float fleeSpeed = 5.0f;
    public float detectionRadius = 5.0f;
    public float directionChangeInterval = 1.0f;
    public float wallAvoidanceDistance = 2.0f;

    private static int currentRatIndex = 0; 
    private static int totalRats = 0; 

    private GameObject player;
    private Rigidbody2D rb;
    private float directionChangeTimer;
    private Vector2 direction;
    private int myIndex;

    void OnEnable()
    {
        currentRatIndex = 0;
        totalRats = 0;
    }
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        directionChangeTimer = directionChangeInterval;

        myIndex = totalRats; 
        totalRats++; 
    }

    void FixedUpdate()
    {
        if (myIndex != currentRatIndex)
        {
            return;
        }

        directionChangeTimer -= Time.fixedDeltaTime;

        if (directionChangeTimer <= 0 || IsWallInDirection(direction))
        {
            do
            {
                direction = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized;
            }
            while (IsWallInDirection(direction));

            directionChangeTimer = directionChangeInterval;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);

        if (distanceToPlayer <= detectionRadius)
        {
            direction = (transform.position - player.transform.position).normalized;
        }

        rb.velocity = direction * (distanceToPlayer <= detectionRadius ? fleeSpeed : wanderSpeed);

        currentRatIndex = (currentRatIndex + 1) % totalRats;
    }

    bool IsWallInDirection(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, wallAvoidanceDistance);
        return hit.collider != null && hit.collider.gameObject.CompareTag("Wall");
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            direction = -direction;
        }
    }
}