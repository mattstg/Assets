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
        strength = GV.PHEROMONE_START_ENERGY;
        if (pHome && pAway)
            gameObject.AddComponent<DrawLineSprite>().Initialize(pHome.transform.position,pAway.transform.position,Resources.Load<Sprite>("Sprites/PheromoneNode"));
    }

    public void GetUpdated()
    {
        strength -= 1;
        GetComponentInChildren<TextMesh>().text = strength.ToString();
        if (strength <= 0)
            TrailDie();
    }

    public void SplitByNode(PheromoneNode newNode)
    {
        FindObjectOfType<PheromoneManager>().CreatePheromoneTrail(pHome, newNode, pherType);
        pHome = newNode;
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

    private void TrailDie()
    {
        if (pHome)
            pHome.PheromoneTrailDied(this);
        if (pAway)
            pAway.PheromoneTrailDied(this);
        FindObjectOfType<PheromoneManager>().DeleteTrail(this);

    }

   

}

