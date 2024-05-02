using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Vector2 posicion;
    public float velocity = 0.5f;
    public GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];

        Debug.Log(posicion);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float x = player.transform.position.x;
        float y = player.transform.position.y;
        posicion = new Vector2(x,y);
        if(Input.GetKey(KeyCode.W)){
            posicion.y += velocity;
        }
        
        if(Input.GetKey(KeyCode.S)){
            posicion.y -= velocity;
        }
        
        if(Input.GetKey(KeyCode.A)){
            posicion.x -= velocity;
        }
        
        if(Input.GetKey(KeyCode.D)){
            posicion.x += velocity;
        }
        player.transform.position = posicion;
        // player.transform.Translate(posicion);

    }
}
