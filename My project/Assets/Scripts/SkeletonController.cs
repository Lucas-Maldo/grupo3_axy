using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject arrow;
    GameObject[] arrows;
    GameObject skeleton;
    private float interval = 1f;
    private float startTime;
    private float elapsedTime;

    void Start(){
        startTime = Time.time;
        skeleton = GameObject.FindGameObjectsWithTag("Skeleton")[0];
    }

    void Update(){
        elapsedTime = Time.time - startTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(elapsedTime >= interval){
            startTime = Time.time;
            Vector3 randomPosition = 3 * new Vector3(Random.Range(-3f, 4f), Random.Range(-3f, 4f), 0).normalized;
            Vector2 randomSpawnPosition = skeleton.transform.position + randomPosition;
            Debug.Log(randomPosition);
            Instantiate(arrow, randomSpawnPosition, Quaternion.identity);
        }
        arrows = GameObject.FindGameObjectsWithTag("Arrow");
        if(arrows.Length > 1){
            Destroy(arrows[0]);
        }

    }
}