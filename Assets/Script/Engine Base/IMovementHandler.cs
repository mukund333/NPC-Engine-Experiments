using UnityEngine;

public interface IMovementEngineController 
{
    public void SetTargetGear(GearState gearState);
    public void SetDirection(Vector2 desiredDirection);
}
