using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementEngineController : MonoBehaviour, IMovementEngineController
{
   public IGearController gearController;
   public ITurnController turnController;
   [SerializeField] private Vector2 direction;
   [SerializeField] private GearState gear;

    private void Awake()
    {
        gearController = GetComponent<IGearController>();
        turnController = GetComponent<ITurnController>();
    }

    private void Update()
    {
        //Debug.Log("desiredDirection :" + direction+" "+ gear);
    }
    public void SetTargetGear(GearState gearState)
    { 
         gearController.SetTargetGear(gearState);
        gear = gearState;
    }

    public void SetDirection(Vector2 desiredDirection)
    {
        turnController.CalculateDesiredAngle(desiredDirection);
        direction = desiredDirection;
    }

   
}
