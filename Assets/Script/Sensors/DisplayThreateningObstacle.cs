using UnityEngine;

public class DisplayThreateningObstacle : MonoBehaviour
{
    public FieldOfView fieldOfView; // Reference to the FieldOfView component

    private void Update()
    {
        // Get the most threatening obstacle from the FOVCore
        DetectedObject mostThreateningObstacle = fieldOfView.GetMostThreateningObstacle();

        if (mostThreateningObstacle != null)
        {
            // Display the position of the most threatening obstacle
            Debug.Log("Most Threatening Obstacle Position: " + mostThreateningObstacle.Target.transform.position);
        }
        else
        {
            Debug.Log("No threatening obstacle detected.");
        }
    }
}
