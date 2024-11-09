using System.Collections.Generic;

public interface IObstacleDetectionSystem
{
    List<Obstacle_Struct> GetDetectedObstacles();
}