using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// This is a simple toy, no need for resource or state control. Just set references and laod the scene. 
/// it's a 2D/ortho only application.
/// Notes:

/// Boid code uses Unity lib for vector math but can be easily adapted to pure .net for use in other c# apps.
/// 
/// Instead of a simple coh, sep, ali boid model I figured it might be fun to make an extendable model.
/// Boids could interract through Drivers however currently all boids share a single set of drivers so make sure
/// to create instances and assign them individually to get individual behaviors.
/// - See: IBoidDriver
/// 
/// All influences / driver values are normalised before being multiplied by power.
/// So, no magic numbers (yay!) are required for a balance in motion, just a uniform view-scale.
/// 
/// No collision, but easy to implement. 
/// - it uses time-steps so should be easy to integrate into Box2d without breaking determinism.
/// 
/// TODO:
/// GetBoidsInRange doesn't account for  boids that are close to the opposite edge (boids are wrapped to 
/// screen space, not killed and spawned... portals!) so they can get grouped.
/// 
/// Sorting would work better using a quadtree adress system.
/// 
/// </summary>
public class TheBoids : MonoBehaviour
{

    // set these in editor
    public Panel_GameUI gameUI;
    public BoidViewManager boidViewManager;
    public Camera boidCamera;

    public float scale = 0.1f; // Scale of the graphic in screen space, used for step calc

    private BoidManager _boidManager;
    private FlockDriver _flockDriver; // AKA cohesion
    private AlignDriver _alignDriver;
    private SeperateDriver _seperateDriver;
    private PointAttractDriver _pointAttractDriver; // makes boids chase the cursor (or whatever)

    private void Awake()
    {

        gameUI.onCoherenceValueChange += HandleCoherenceValueChange;
        gameUI.onSeperationValueChange += HandleSeperationValueChange;
        gameUI.onAlignmentValueChange += HandleAlignmentValueChange;

        _boidManager = new BoidManager();
        boidViewManager.boidManager = _boidManager;

        _boidManager.count = 50;

        _flockDriver = new FlockDriver();
        _alignDriver = new AlignDriver();
        _seperateDriver = new SeperateDriver();

        _pointAttractDriver = new PointAttractDriver(); // To make the boids follow a coursor or whatnot, See: Update()

        foreach (Boid boid in _boidManager.Boids)
        {
            boid.drivers.Add(_flockDriver);
            boid.drivers.Add(_alignDriver);
            boid.drivers.Add(_seperateDriver);
            boid.drivers.Add(_pointAttractDriver);
        }
        
        // set initial values
        HandleCoherenceValueChange(gameUI.slider_Coherence.value);
        HandleSeperationValueChange(gameUI.slider_Seperation.value);
        HandleAlignmentValueChange(gameUI.slider_Alignment.value);

    }

    private void HandleCoherenceValueChange(float val)
    {
        _flockDriver.weight = val * scale;
    }

    private void HandleSeperationValueChange(float val)
    {
        _seperateDriver.weight = val * scale;
    }

    private void HandleAlignmentValueChange(float val)
    {
        _alignDriver.weight = val * scale;
    }

    private void Update()
    {

        // step the boid sim using time since last frame
        _boidManager.StepBoids(Time.deltaTime);

        // Set point attractor values
        if (Input.GetMouseButton(0) && 
            EventSystem.current.currentSelectedGameObject == null &&
            EventSystem.current.IsPointerOverGameObject(0) == false)
        {

            Vector3 pointPos = boidCamera.ScreenToWorldPoint(Input.mousePosition);
            _pointAttractDriver.point = new Vector2(pointPos.x, pointPos.y);
            _pointAttractDriver.weight = scale;

        }
        else
            _pointAttractDriver.weight = 0;
       
    }

    private void OnDestroy()
    {

        gameUI.onCoherenceValueChange -= HandleCoherenceValueChange;
        gameUI.onSeperationValueChange -= HandleSeperationValueChange;
        gameUI.onAlignmentValueChange -= HandleAlignmentValueChange;

    }

}