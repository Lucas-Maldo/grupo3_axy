using UnityEngine;
using UnityEditor;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource;

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "BlindGazer" || collision.gameObject.tag == "Skeleton")
        {
            Debug.Log("Collided with: " + collision.gameObject.tag);
            audioSource.Play();
        }
    }
}