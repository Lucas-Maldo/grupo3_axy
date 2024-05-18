using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

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

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Arrow" || collision.gameObject.tag == "BlindGazer" || collision.gameObject.tag == "Skeleton")
        {
            Debug.Log("Collided with: " + collision.gameObject.tag);
            //end the game or restart the game
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        }
        
        if (collision.gameObject.tag == "Exit") 
        {
            WinGame();
        }
    }

    void WinGame()
    {
        Debug.Log("¡Has ganado!");
    }   
}