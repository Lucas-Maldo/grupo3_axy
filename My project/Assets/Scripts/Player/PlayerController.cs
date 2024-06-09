using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour, IObserver
{
    
    public InputController Input; 
    public PhysicsController Physics; 
    // public StateController States; 
    public AudioController Audio; 
    public GameObject player;
    public float velocity;
    public bool gameEnd = false;
    private float interval = 0.5f;
    private float startTime;

    void Start()
    {
        Input = this.GetComponent<InputController>();
        Physics = this.GetComponent<PhysicsController>();
        // States = this.GetComponent<StateController>();
        Audio = this.GetComponent<AudioController>();
        InitializePlayer();
        GlobalListener.instance.RegisterObserver(this);
    }

    void Update()
    {
        Vector2 direction = Input.Update();
        Physics.Movement(direction, velocity, player);
        if(Time.time - startTime >= interval && gameEnd){
            GlobalListener.instance.NotifyLose();
        }
    }

    void InitializePlayer()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Arrow" || collision.gameObject.tag == "BlindGazer" || collision.gameObject.tag == "Skeleton" ||  collision.gameObject.tag == "HungryZombie")
        {
            Debug.Log("Collided with: " + collision.gameObject.tag);
            startTime = Time.time;
            gameEnd = true;
            GlobalListener.instance.NotifyAudio();
        }
        
        if (collision.gameObject.tag == "Exit") 
        {
            GlobalListener.instance.NotifyWin();
        }
    }
    public void OnNotifyWin()
    {
        Debug.Log("¡Has ganado!");
    }
    public void OnNotifyLose()
    {
        Debug.Log("¡Has perdido!");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void OnNotifyAudio()
    {
        Audio.playAudio();
    }
}