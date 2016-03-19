﻿using UnityEngine;
using System.Collections;

public class PheromoneTrail : MonoBehaviour {
    GV.PhermoneTypes pherType;
    PheromoneNode pHome;
    PheromoneNode pAway;
    float strength;

    public void Initialize(PheromoneNode _home, PheromoneNode _away,GV.PhermoneTypes _pherType)
    {
        pherType = _pherType;
        pHome = _home;
        pAway = _away;
        strength = 1;
    }

    public void GetUpdated()
    {
        strength -= 1;
        DrawRenderer();  //SHOULD ONLY BE CALLED WHEN CHANGED OR MOVED
    }

    ///Graphics section
    private void DrawRenderer()
    {
        if (pHome && pAway)
        {
            GetComponent<LineRenderer>().SetPosition(0, pHome.transform.position);
            GetComponent<LineRenderer>().SetPosition(1, pAway.transform.position);
        }
    }

    private void TrailDie()
    {
        if (pHome)
            pHome.DeletePheromoneTrail(this);
        if (pAway)
            pAway.DeletePheromoneTrail(this);
    }

}
