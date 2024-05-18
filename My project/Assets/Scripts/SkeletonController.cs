using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    public ShootingScipt shooting;
    public TimingScript timing;

    private void Start()
    {
        shooting = this.GetComponent<ShootingScipt>();
        timing = this.GetComponent<TimingScript>();
    }
}