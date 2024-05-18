using UnityEngine;

public class BlindGazer : MonoBehaviour
{
    public MovementController movementController;
    public StateController stateController;

    public bool moveUp;

    void Start()
    {
        movementController = transform.GetComponent<MovementController>();
        stateController = transform.GetComponent<StateController>();
    }

    void Update()
    {
        moveUp = stateController.CheckDirection(transform.position);
        movementController.MoveEnemy(moveUp);
    }
}