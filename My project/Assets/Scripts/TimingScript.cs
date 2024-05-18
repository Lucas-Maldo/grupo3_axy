using UnityEngine;

public class TimingScript : MonoBehaviour
{
    public ShootingScipt shootingComponent;
    private float interval = 1f;
    private float elapsedTime;

    private void Update()
    {
        UpdateElapsedTime();
    }

    private void UpdateElapsedTime()
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= interval)
        {
            elapsedTime = 0f;
            shootingComponent.ArrowSpawn();
        }
        else{
        shootingComponent.ArrowDestroy();

        }
    }
}