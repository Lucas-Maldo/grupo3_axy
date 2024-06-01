using UnityEngine;

public class BlindGazer : MonoBehaviour
{
    public MovementController movementController;
    public StateController stateController;

    public bool moveUp;

    void Start()
    {
        movementController = this.GetComponent<MovementController>();
        stateController = this.GetComponent<StateController>();
    }

    void Update()
    {

        moveUp = stateController.CheckDirection(transform.position);
        movementController.MoveEnemy(moveUp);
    }
}