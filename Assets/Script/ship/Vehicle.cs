using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Vehicle : MonoBehaviour
{

    ///carful IMoveHandler in unity base interface

    IMovementEngineController movementEngineController;

    [SerializeField] GearState gearState;

    public Transform target;

    IGearController gearController;
    private void Start()
    {
        movementEngineController = GetComponent<IMovementEngineController>();

        movementEngineController.SetTargetGear(gearState);

        gearController = GetComponent<IGearController>();   
    }
    private void FixedUpdate()
    {
     
        
      Vector2 direction = (target.position - transform.position).normalized;

       float distance = Vector2.Distance(transform.position, target.position);

        //if (distance <=5)
        //{
        //    gearState = GearState.Neutral; // Vehicle stopped

        //}else if (distance <15f && distance > 5f)
        //{
        //    gearState = GearState.First;
        //}
       
        //movementEngineController.SetTargetGear(gearState);
        movementEngineController.SetDirection(direction);

        //Debug.Log("Distance: " + distance + " | Current Gear State: " + gearController.GetCurrentGear());

    }
}
