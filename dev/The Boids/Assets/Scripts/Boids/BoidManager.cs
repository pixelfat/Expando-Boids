using System.Collections.Generic;
using UnityEngine;

public class BoidManager
{

    public delegate void BoidManagerEvent(Boid boid);
    public BoidManagerEvent
        OnBoidAdded,
        OnBoidRemoved;

    ///TODO: remove magic numbers and use camera win/max
    public float
        minX = -2.5f,
        minY = -5f,
        maxX = 2.5f,
        maxY = 5f;

    public Boid[] Boids { get { return _boids.ToArray(); } }

    public int count { get { return _boids.Count; } set { SetCount(value); } }

    private List<Boid> _boids = new List<Boid>();

    private void SetCount(int newCount)
    {

        if (newCount > _boids.Count)
        {

            while (newCount > _boids.Count)
                AddBoid();

        }
        else
            while (newCount < _boids.Count)
                RemoveBoid(_boids[0]);

    }

    private void AddBoid()
    {

        Vector2 startVel = new Vector2(
            (Random.value * 2) - 1,
            (Random.value * 2) - 1);

        Vector2 startPos = new Vector2(
            ((Random.value * 2) - 1) * (maxX - minX),
            (Random.value * 2) - 1) * (maxY - minY);

        Boid newBoid = new Boid(this, startPos, startVel);

        _boids.Add(newBoid);

        OnBoidAdded?.Invoke(newBoid);

    }

    private void RemoveBoid(Boid boid)
    {

        _boids.Remove(boid);

        OnBoidRemoved?.Invoke(boid);

    }
    
    public void StepBoids(double stepTime)
    {

        foreach (Boid boid in _boids)
            boid.DoStep(stepTime);

    }

}