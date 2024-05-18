using UnityEngine;

public class ShootingScipt : MonoBehaviour
{
    public GameObject arrow;
    private GameObject skeleton;

    private void Start()
    {
        skeleton = GameObject.FindGameObjectsWithTag("Skeleton")[0];
    }

    public void ArrowSpawn()
    {
        Vector3 randomPosition = 3 * new Vector3(Random.Range(-3f, 4f), Random.Range(-3f, 4f), 0).normalized;
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