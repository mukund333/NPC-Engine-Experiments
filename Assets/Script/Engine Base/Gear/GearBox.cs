using System.Collections;
using UnityEngine;

//public enum GearState { Neutral, First, Second, Third }



public class Gearbox : MonoBehaviour, IGearController
{
   

    [SerializeField] private GearSystem gearSystem;
    [SerializeField] private float gearShiftDelay = 0.2f; // Adjust the delay as needed

    [SerializeField] private GearState targetGearState;
    [SerializeField] private GearState currentGearState;

    private bool isShifting = false; // Flag to track if a gear shift is in progress

    private void Awake()
    {
        gearSystem = GetComponentInParent<GearSystem>();
    }

    private void Update()
    {
        HandleGearShifts();
        //Debug.Log(" Current Gear State: " + currentGearState);
    }


    private void HandleGearShifts()
    {
        if (currentGearState != targetGearState && !isShifting)
        {
            StartCoroutine(ShiftGearWithDelay());
        }
    }

    private IEnumerator ShiftGearWithDelay()
    {
        isShifting = true; // Set the flag to indicate a gear shift is in progress

        yield return new WaitForSeconds(gearShiftDelay); // Wait for the specified delay

        int targetGear = (int)targetGearState;
        int currentGear = (int)currentGearState;

        if (targetGear > currentGear)
        {
            gearSystem.GearShiftUp(); // Shift up
        }
        else if (targetGear < currentGear)
        {
            gearSystem.GearShiftDown(); // Shift down
        }

        currentGearState = (GearState)gearSystem.currentGear;
        isShifting = false; // Reset the flag after the shift is completed
  
    }

    public void SetTargetGear(GearState gearState)
    {
        targetGearState = gearState;
    }

    public GearState GetCurrentGear()
    {
        return currentGearState;
    }
}
