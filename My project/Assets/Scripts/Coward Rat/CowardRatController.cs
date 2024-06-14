using System;
using UnityEngine;
using UnityEngine.UIElements;

public class CowardRatController : MonoBehaviour
{
    public float wanderSpeed = 2.0f;
    public float attackSpeed = 3.5f;
    public float fleeSpeed = 5.0f;
    public float detectionRadius = 5.0f;
    public float directionChangeInterval = 1.0f;
    public float wallAvoidanceDistance = 2.0f;

    private static int currentRatIndex = 0; 
    private static int totalRats = 0; 

    private GameObject player;
    private Rigidbody2D rb;
    private string background_status;
    private float directionChangeTimer;
    private Vector2 direction;
    private int myIndex;
    
    class DecisionNode
    {
        public Func<bool> Condition { get; set; }
        public Action TrueAction { get; set; }
        public Action FalseAction { get; set; }
    }
    DecisionNode root;
    
    void OnEnable()
    {
        currentRatIndex = 0;
        totalRats = 0;
    }
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;

        background_status = GameObject.FindGameObjectWithTag("Background").GetComponent<BackgroundChanger>().status;

        directionChangeTimer = directionChangeInterval;

        root = new DecisionNode
        {
            Condition = () => Vector2.Distance(transform.position, player.transform.position) <= detectionRadius,
            TrueAction = () => Flee(),
            FalseAction = () => Idle()
        };

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

        // Traverse(root);

        currentRatIndex = (currentRatIndex + 1) % totalRats;
        Debug.Log(background_status);
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
    void Traverse(DecisionNode node)
    {
        if (node.Condition())
        {
            node.TrueAction(); //Flee or attack
        }
        else
        {
            node.FalseAction(); //Idle
        }
    }

    void Flee() {
        if (background_status == "Night")
        {
            direction = (transform.position - player.transform.position).normalized;
            rb.velocity = direction * attackSpeed;
        }
        else if (background_status == "Day")
        {
            direction = (transform.position - player.transform.position).normalized;
            rb.velocity = direction * fleeSpeed;
        }
    }

    void Idle() {
        if (directionChangeTimer <= 0 || IsWallInDirection(direction))
        {
            direction = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
            directionChangeTimer = directionChangeInterval;
            rb.velocity = direction * wanderSpeed;
        }
    }
}