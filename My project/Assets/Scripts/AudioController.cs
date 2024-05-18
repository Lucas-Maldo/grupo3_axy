using UnityEngine;
using UnityEditor;
using System.Collections;
[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip impact;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Wall" || collision.gameObject.tag == "BlindGazer" || collision.gameObject.tag == "Skeleton")
        {
            Debug.Log("Collided with: " + collision.gameObject.tag);
            // audioSource.PlayOneShot();
            audioSource.PlayOneShot(impact, 0.7F);
        }
    }
}