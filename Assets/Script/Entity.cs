using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] ContextSteeringManager contextSteeringManager;
    [SerializeField] IMovementEngineController movementEngineController;

    private void Start()
    {
        contextSteeringManager = GetComponent<ContextSteeringManager>();
        movementEngineController = GetComponent<IMovementEngineController>();

    }

    private void Update()
    {
        SetDirection(contextSteeringManager.GetBestDirection());
    }

   

    

    private void SetDirection(Vector2 direction)
    {
        movementEngineController.SetDirection(direction);
        
    }
}
