using UnityEngine;

public interface IBoidDriver
{
    string name { get; }
    float weight { get; set; }
    Vector2 GetValue(Boid boid);

}

