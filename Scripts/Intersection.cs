using UnityEngine;
using System.Collections;

public class Intersection {
    public Vector2 _intersectionPoint;
    public PheromoneTrail _intersectionTrail;
    public Intersection(Vector2 _ip, PheromoneTrail _it)
    {
        _intersectionPoint = _ip;
        _intersectionTrail = _it;
    }

}
