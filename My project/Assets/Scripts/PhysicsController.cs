using UnityEngine;
using UnityEditor;

public class PhysicsController : MonoBehaviour
{
    private Vector2 position;

    public void Movement(Vector2 direction, float velocity, GameObject player)
    {
        position = direction.normalized;
        player.transform.Translate(Time.deltaTime * velocity * position);
    }
}