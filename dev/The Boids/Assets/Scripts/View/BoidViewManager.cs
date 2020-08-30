using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BoidViewManager : MonoBehaviour
{

    public BoidManager boidManager;
    public BoidView boidViewPrefab;

    public Dictionary<Boid, BoidView> boidViews = new Dictionary<Boid, BoidView>();

    private void Start()
    {

        boidViewPrefab.gameObject.SetActive(false);

        boidManager.OnBoidAdded += HandleBoidAdded;
        boidManager.OnBoidRemoved += HandleBoidRemoved;

        foreach (Boid boid in boidManager.Boids)
            HandleBoidAdded(boid);

    }

    private void OnDestroy()
    {

        boidManager.OnBoidAdded -= HandleBoidAdded;
        boidManager.OnBoidRemoved -= HandleBoidRemoved;

        foreach (Boid boidKey in boidViews.Keys.ToArray())
            HandleBoidRemoved(boidKey);

    }

    private void HandleBoidAdded(Boid boid)
    {

        BoidView newBoidView = Instantiate(boidViewPrefab);
        newBoidView.transform.SetParent(transform);
        newBoidView.boid = boid;
        newBoidView.gameObject.SetActive(true);

        boidViews.Add(boid, newBoidView);

    }

    private void HandleBoidRemoved(Boid boid)
    {

        Destroy(boidViews[boid]);
        boidViews.Remove(boid);

    }

}