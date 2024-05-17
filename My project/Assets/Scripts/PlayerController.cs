using UnityEngine;
using UnityEditor;

public class PlayerController : MonoBehaviour
{
    public InputController Input; 
    public PhysicsController Physics; 
    // public StateController States; 
    public AudioController Audio; 
    public GameObject player;
    public float velocity;

    void Start()
    {
        Input = this.GetComponent<InputController>();
        Physics = this.GetComponent<PhysicsController>();
        // States = this.GetComponent<StateController>();
        Audio = this.GetComponent<AudioController>();
        InitializePlayer();
    }

    void Update()
    {
        Vector2 direction = Input.Update();
        Physics.Movement(direction, velocity, player);
    }

    void InitializePlayer()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }
}