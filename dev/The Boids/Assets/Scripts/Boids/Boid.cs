using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Boid
{

    public HashSet<IBoidDriver> drivers = new HashSet<IBoidDriver>();

    public Vector2 Velocity { get; private set; } = Vector2.zero;
    public Vector2 Position { get; private set; } = Vector2.zero;

    public float maxSpeed = 0.01f;
    public float radiusOfInfluence = 1.5f;

    public BoidManager Manager { get; }

    /// <summary>
    /// Manage boid count and stepping. 
    /// </summary>
    /// <param name="mngr">The (flock) manager. Contains utils for driver functions.</param>
    /// <param name="pos">Initial position</param>
    /// <param name="vel">Initial velocity</param>
    public Boid(BoidManager mngr, Vector2 pos, Vector2 vel)
    {

        Manager = mngr;
        Position = pos;
        Velocity = vel;

    }

    public void DoStep(double stepDelta)
    {

        // add driver values
        foreach (IBoidDriver driver in drivers)
            Velocity += driver.GetValue(this) * (float)stepDelta;

        // cap speed
        if (Velocity.magnitude > maxSpeed)
            Velocity = Velocity.normalized * maxSpeed;

        Position += Velocity;

        // wrap coords (telepoprt out of bounds coords to other side)
        if (Position.x > Manager.maxX)
            Position = new Vector2(Manager.minX + (Position.x - Manager.maxX), Position.y);

        if (Position.x < Manager.minX)
            Position = new Vector2(Manager.maxX, Position.y);

        if (Position.y > Manager.maxY)
            Position = new Vector2(Position.x, Manager.minY + (Position.y - Manager.maxY));

        if (Position.y < Manager.minY)
            Position = new Vector2(Position.x, Manager.maxY);

    }

    /// <summary>
    /// This is NOT the rotation (boids don't have one) it's just a hacky way to face the direction of
    /// the current boid velocity.
    /// </summary>
    /// <returns></returns>
    public float VelocityAngle()
    {

        float angle = Mathf.Atan2(Velocity.y, Velocity.x);
        angle *= (180 / Mathf.PI);

        return angle;

    }

    /// <summary>
    /// Gets all boids within radiusOfInfluence.
    /// TODO: Maybe implement some spacial sorting or simple quadtree address lookup. 50 agents isn't enough to worry about..
    /// </summary>
    /// <returns></returns>
    public Boid[] GetBoidsInRange()
    {

        return Manager.Boids.Where(x => x.DistanceTo(this) < radiusOfInfluence && x != this).ToArray();

    }

    public float DistanceTo(Boid boid)
    {
        // TODO: Account for wrapping
        return (boid.Position - Position).magnitude;
    }

}
