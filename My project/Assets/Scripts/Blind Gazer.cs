using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlindGazer : MonoBehaviour
{
    

    // Posisiones
    private float startX = 2.0f;
    private float startY = -3.0f;
    private float endY = 3.0f;

    // Movimiento
    public bool moveUp = true; 
    public float speed = 1.0f;

        void Update()
    {
        // Movimiento Enemigo
        if (moveUp)
        {
            transform.position = new Vector2(startX, transform.position.y + speed * Time.deltaTime);
        }
        else
        {
            transform.position = new Vector2(startX, transform.position.y - speed * Time.deltaTime);
        }

       // Cambio Direccion
        if (transform.position.y >= endY)
        {
            moveUp = false;
        }
        else if (transform.position.y <= startY)
        {
            moveUp = true;
        }
    }
}