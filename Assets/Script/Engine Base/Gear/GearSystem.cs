using UnityEngine;

public class GearSystem : MonoBehaviour, IGearSystem
{
    [SerializeField] private bool[] gears = new bool[4];
    public int currentGear;
    [SerializeField] private float[] gearSpeeds = new float[4];
    private float delay = 0;//0.01f;

    private void Start()
    {
        gearSpeeds[0] = 0f;
        gearSpeeds[1] = 1f;
        gearSpeeds[2] = 5f;
        gearSpeeds[3] = 7f;
        ResetGears();
    }

    public void GearShiftUp()
    {
        if (currentGear < gears.Length - 1)
        {
            gears[currentGear] = false;
            currentGear++;
            gears[currentGear] = true;
            Invoke("AccelerateAfterGearShift", delay);
        }
    }

    public void GearShiftDown()
    {
        if (currentGear > 0)
        {
            gears[currentGear] = false;
            currentGear--;
            gears[currentGear] = true;
            Invoke("DecelerateAfterGearShift", delay);//delay for sudden execution
        }
    }

    public float GetMaxSpeed()
    {
        return gearSpeeds[currentGear];
    }

    public int GetCurrentGear()
    {
        return currentGear;
    }

    private void AccelerateAfterGearShift()
    {
        GetComponent<IThrustControl>().Accelerate();
    }

    private void DecelerateAfterGearShift()
    {
        GetComponent<IThrustControl>().Decelerate();
    }

    private void ResetGears()
    {
        for (int i = 0; i < gears.Length; i++) gears[i] = false;
        gears[0] = true;
        currentGear = 0;
    }
}
