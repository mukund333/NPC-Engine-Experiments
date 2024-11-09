using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class TurnMechanics : MonoBehaviour, ITurnController
{
    [SerializeField] float maxTurn;
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] float currentRotationSpeed;
    [SerializeField] AnimationCurve torqueCurve;



    private void Start()
    {
    
        currentRotationSpeed = 0.01f;
        rb2d = GetComponent<Rigidbody2D>();
        rb2d.angularDrag = 12f;///prevent Oscillation
    }

    //combined Force 
    public void CalculateDesiredAngle(Vector2 desiredDirection)
    {
        desiredDirection.Normalize();
        float desiredAngle = Mathf.Atan2(desiredDirection.y, desiredDirection.x) * Mathf.Rad2Deg - 90f;
        ApplyTorque(desiredAngle);
    }

    public void ApplyTorque(float desiredAngle)
    {
        float currentAngle = rb2d.rotation;
        float angleDifference = Mathf.DeltaAngle(currentAngle, desiredAngle);

        currentRotationSpeed *= Time.fixedDeltaTime;

        float torqueMultiplier = torqueCurve.Evaluate(currentRotationSpeed / maxTurn);
        float torque = angleDifference * maxTurn * torqueMultiplier * Time.fixedDeltaTime;


        rb2d.AddTorque(torque);
    }

}
