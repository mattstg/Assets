using UnityEngine;
using System.Collections;

public class PheromoneTrail : MonoBehaviour {
    GV.PhermoneTypes pherType;
    public PheromoneNode pHome;
    public PheromoneNode pAway;
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
        ValidateTrail();
    }

    public void SplitByNode(PheromoneNode newNode)
    {
        FindObjectOfType<PheromoneManager>().CreatePheromoneTrail(pHome, newNode, pherType);
        pHome = newNode;
    }

    public void SetNewNode(PheromoneNode oldNode, PheromoneNode newNode)
    {
        if (!oldNode || !newNode)
        {
            TrailDie();
            return;
        }

        if (pHome == oldNode)
            pHome = newNode;
        if (pAway == oldNode)
            pAway = newNode;
        if (pHome && pAway)
            gameObject.GetComponent<DrawLineSprite>().Initialize(pHome.transform.position, pAway.transform.position, Resources.Load<Sprite>("Sprites/PheromoneNode"));
    }

    //patch fix
    public void ValidateTrail()
    {
        if (!pHome || !pAway || strength <= 0)
            TrailDie();
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

    //returns the copy of pher found, the one that will become absorbed
    public static PheromoneTrail PherListContains(System.Collections.Generic.List<PheromoneTrail> pherList, PheromoneTrail toFind)
    {
        foreach (PheromoneTrail pt in pherList)
            if (PherTrailIsEqual(pt, toFind.pHome, toFind.pAway))
                return pt;
        return null;
    }

    public static bool PherTrailIsEqual(PheromoneTrail p1, PheromoneNode pn1, PheromoneNode pn2)
    {
        if ((p1.pHome == pn1 || p1.pHome == pn2) && (p1.pAway == pn1 || p1.pAway == pn2))
        {
            Debug.Log("freaking sf");
            return true;
        }
        Debug.Log("no equal trail found");
        return false;
    }
    

}

