using System.Linq;
using UnityEngine;

public class FlockDriver : IBoidDriver
{
    public string name { get; } = "Flock Driver";

    public float weight { get; set; } = 0;

    public Vector2 GetValue(Boid boid)
    {

        Boid[] boidsInRange = boid.GetBoidsInRange();

        if (boidsInRange.Length == 0)
            return Vector2.zero;

        float avgX = boidsInRange.Sum(x => x.Position.x) / boidsInRange.Length;
        float avgY = boidsInRange.Sum(x => x.Position.y) / boidsInRange.Length;

        return new Vector2(avgX - boid.Position.x, avgY - boid.Position.y) * weight;

    }

}

public class SeperateDriver : IBoidDriver
{
    public string name { get; } = "Seperate Driver";

    public float weight { get; set; } = 0;

    public Vector2 GetValue(Boid boid)
    {

        Boid[] boidsInRange = boid.GetBoidsInRange();

        if (boidsInRange.Length == 0)
            return Vector2.zero;

        Vector2 rtrnVal = Vector2.zero;

        foreach (var boidOfInfluence in boidsInRange)
            rtrnVal += boid.Position - boidOfInfluence.Position;

        return rtrnVal.normalized * weight;

    }

}

public class AlignDriver : IBoidDriver
{

    public string name { get; } = "Align Driver";

    public float weight { get; set; } = 0;

    public Vector2 GetValue(Boid boid)
    {

        Boid[] boidsInRange = boid.GetBoidsInRange();

        if (boidsInRange.Length == 0)
            return Vector2.zero;

        // use the avg of all velocities
        Vector2 AvgVel = new Vector2(
            boidsInRange.Sum(x => x.Velocity.x) / boidsInRange.Length,
            boidsInRange.Sum(x => x.Velocity.y) / boidsInRange.Length);

        return AvgVel.normalized * weight;

    }

}

public class PointAttractDriver : IBoidDriver
{

    public string name { get; } = "Point Attract Driver";

    public float weight { get; set; } = 0f;

    public Vector2 point = Vector2.zero;

    public Vector2 GetValue(Boid boid)
    {

        return (point - boid.Position).normalized * weight;

    }

}
