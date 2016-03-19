using UnityEngine;
using System.Collections;

public class PheromoneTrail : MonoBehaviour {
    GV.PhermoneTypes pherType;
    PheromoneNode pHome;
    PheromoneNode pAway;
    public int strength;

    public void Initialize(PheromoneNode _home, PheromoneNode _away,GV.PhermoneTypes _pherType)
    {
        pherType = _pherType;
        pHome = _home;
        pAway = _away;
        strength = 10;
    }

    public void GetUpdated()
    {
        strength -= 1;
        DrawRenderer();  //SHOULD ONLY BE CALLED WHEN CHANGED OR MOVED
        if (strength <= 0)
            TrailDie();
    }

    public void SetNewNode(PheromoneNode oldNode, PheromoneNode newNode)
    {
        if (pHome == oldNode)
            pHome = newNode;
        if (pAway == oldNode)
            pAway = newNode;
    }

    public PheromoneNode GetOtherNode(PheromoneNode otherNode)
    {
        return (otherNode == pHome) ? pAway : pHome;
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
            pHome.PheromoneTrailDied(this);
        if (pAway)
            pAway.PheromoneTrailDied(this);
        FindObjectOfType<PheromoneManager>().DeleteTrail(this);

    }

   

}

