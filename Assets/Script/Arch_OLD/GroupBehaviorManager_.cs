using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class GroupBehaviorManager_ : MonoBehaviour
{
    public int numSlots = 36;              // Number of slots in the context map (resolution)
    public int falloffRange = 3;           // Range for applying falloff
    public float moveSpeed = 2f;           // Movement speed for each entity
    public float cohesionWeight = 1.0f;    // Weight for cohesion behavior
    public float separationWeight = 1.0f;  // Weight for separation behavior
    public float alignmentWeight = 1.0f;   // Weight for alignment behavior
    public float separationRadius = 2.0f;  // Radius for separation behavior

    private List<Transform> groupEntities = new List<Transform>(); // List of all entities in the group

    void Start()
    {
        // Find all entities tagged as "GroupEntity"
        groupEntities = GameObject.FindGameObjectsWithTag("GroupEntity").Select(go => go.transform).ToList();
    }

    void Update()
    {
        foreach (Transform entity in groupEntities)
        {
            Vector2 cohesionVector = CalculateCohesion(entity);
            Vector2 separationVector = CalculateSeparation(entity);
            Vector2 alignmentVector = CalculateAlignment(entity);

            // Combine group behavior vectors
            Vector2 groupDirection = cohesionVector * cohesionWeight + separationVector * separationWeight + alignmentVector * alignmentWeight;

            // Normalize the final direction to prevent excessive speed
            groupDirection.Normalize();

            // Apply movement to the entity
            entity.position += (Vector3)(groupDirection * moveSpeed * Time.deltaTime);
        }
    }

    private Vector2 CalculateCohesion(Transform entity)
    {
        // Calculate the center of the group and move towards it
        Vector2 groupCenter = Vector2.zero;
        foreach (var other in groupEntities)
        {
            if (other != entity) groupCenter += (Vector2)other.position;
        }
        groupCenter /= (groupEntities.Count - 1);
        return (groupCenter - (Vector2)entity.position).normalized;
    }

    private Vector2 CalculateSeparation(Transform entity)
    {
        // Calculate a vector to move away from nearby entities
        Vector2 separationVector = Vector2.zero;
        foreach (var other in groupEntities)
        {
            if (other != entity && Vector2.Distance(entity.position, other.position) < separationRadius)
            {
                separationVector += ((Vector2)entity.position - (Vector2)other.position).normalized;
            }
        }
        return separationVector.normalized;
    }

    private Vector2 CalculateAlignment(Transform entity)
    {
        // Calculate the average direction of the group and align with it
        Vector2 averageDirection = Vector2.zero;
        foreach (var other in groupEntities)
        {
            if (other != entity)
            {
                averageDirection += (Vector2)other.up; // Assuming 'up' is the forward direction
            }
        }
        averageDirection /= (groupEntities.Count - 1);
        return averageDirection.normalized;
    }
}
