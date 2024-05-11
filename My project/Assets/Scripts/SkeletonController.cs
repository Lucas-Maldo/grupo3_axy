using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public GameObject arrow;
    GameObject[] arrows;
    GameObject skeleton;
    private float interval = 1f;
    private float elapsedTime;

    void Start(){
        skeleton = GameObject.FindGameObjectsWithTag("Skeleton")[0];
    }

    void Update(){
        elapsedTime += Time.deltaTime;
        if(elapsedTime >= interval){
            elapsedTime = 0f;
            ArrowSpawn();
        }
        ArrowDestroy();
    }

    void ArrowSpawn(){
        Vector3 randomPosition = 3 * new Vector3(Random.Range(-3f, 4f), Random.Range(-3f, 4f), 0).normalized;
        Vector2 randomSpawnPosition = skeleton.transform.position + randomPosition;
        Debug.Log(randomPosition);
        Instantiate(arrow, randomSpawnPosition, Quaternion.identity);
    }

    void ArrowDestroy(){
        arrows = GameObject.FindGameObjectsWithTag("Arrow");
        if(arrows.Length > 1){
            Destroy(arrows[0]);
        }
    }
}