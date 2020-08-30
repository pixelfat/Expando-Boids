using UnityEngine;

public class BoidView : MonoBehaviour
{

    public Boid boid;
    public SpriteRenderer sprite;

    // Update is called once per frame
    public void Update()
    {

        transform.position = new Vector3(boid.Position.x, boid.Position.y, 0);
        transform.rotation = Quaternion.Euler(0, 0, boid.VelocityAngle() - 90);

    }
}
