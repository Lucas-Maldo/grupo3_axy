using Unity.VisualScripting;
using UnityEngine;

public class ShootingScipt : MonoBehaviour
{
    public GameObject arrow;
    private GameObject skeleton;
    public float ArrowSpawnDistance = 3f;

    private void Start()
    {
        skeleton = GameObject.FindGameObjectsWithTag("Skeleton")[0];
    }

    public void ArrowSpawn()
    {
        Vector3 randomPosition = ArrowSpawnDistance * new Vector3(Random.Range(-ArrowSpawnDistance, ArrowSpawnDistance+1), Random.Range(-ArrowSpawnDistance, ArrowSpawnDistance+1f), 0).normalized;
        Vector2 randomSpawnPosition = skeleton.transform.position + randomPosition;
        Instantiate(arrow, randomSpawnPosition, Quaternion.identity);
    }

    public void ArrowDestroy()
    {
        GameObject[] arrows = GameObject.FindGameObjectsWithTag("Arrow");
        if (arrows.Length > 1)
        {
            Destroy(arrows[0]);
        }
    }
}