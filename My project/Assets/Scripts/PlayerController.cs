using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 posicion;
    public float velocity = 5f;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];

        Debug.Log(posicion);
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInput();
    }

    void ProcessInput() {
        float direction_x = 0;
        float direction_y = 0;

        if(Input.GetKey(KeyCode.W)){
            direction_y = 1f;
        }
        if(Input.GetKey(KeyCode.S)){
            direction_y = -1f;
        }
        if(Input.GetKey(KeyCode.A)){
            direction_x = -1f;
        }
        if(Input.GetKey(KeyCode.D)){
            direction_x = 1f;
        }
        posicion = new Vector2(direction_x, direction_y).normalized;
        
        // player.transform.position = posicion;
        player.transform.Translate(posicion * velocity * Time.deltaTime);
    }
}