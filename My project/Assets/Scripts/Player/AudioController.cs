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

    public void playAudio()
    {
        audioSource.PlayOneShot(impact, 0.7F);
    }
}