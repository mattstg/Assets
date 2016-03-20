using UnityEngine;
using System.Collections;

public class PheromoneTrail : MonoBehaviour {
    SpriteRenderer spriteRend;
    public DrawLineSprite drawTrail;
    GV.PhermoneTypes pherType;
    
    public PheromoneNode pHome;
    public PheromoneNode pAway;
    public int _strength = 0;
    public int strength { set { _strength = (value <= GV.PHEROMONE_MAX_ENERGY) ? value : GV.PHEROMONE_MAX_ENERGY; } get { return _strength; } }

    public bool IsValid()
    {
        return pHome && pAway && drawTrail && (strength > 0);
    }
    public void Initialize(PheromoneNode _home, PheromoneNode _away,GV.PhermoneTypes _pherType)
    {
        pherType = _pherType;
        pHome = _home;
        pAway = _away;
        strength = GV.PHEROMONE_START_ENERGY;
        if (pHome && pAway)
            drawTrail.Initialize(pHome.transform.position,pAway.transform.position,"Sprites/PheromoneNode");
    }

    public void Start()
    {
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public void GetUpdated()
    {
		strength -= Mathf.CeilToInt(strength * GV.PATH_DECAY_PCNT) + GV.FLAT_PATH_DECAY;
        GetComponentInChildren<TextMesh>().text = strength.ToString();
        if(!IsValid())
            TrailDie();
        
        
    }

    public void SplitByNode(PheromoneNode newNode)
    {
        FindObjectOfType<PheromoneManager>().CreatePheromoneTrail(pHome, newNode, pherType);
        pHome = newNode;
        drawTrail.Initialize(pHome.transform.position, pAway.transform.position);
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

       /* if (pHome == null || pAway == null)
            Debug.Log("fucking nulls");
		if (gameObject == null || !drawTrail)
            Debug.Log("YOOO");*/

        if (pHome != null && pAway != null)
            drawTrail.Initialize(pHome.transform.position, pAway.transform.position, "Sprites/PheromoneNode");
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

    public void TrailDie()
    {
        if (pHome)
            pHome.PheromoneTrailDied(this);
        if (pAway)
            pAway.PheromoneTrailDied(this);
        FindObjectOfType<PheromoneManager>().DeleteTrail(this);

    }

    //graphics
    public void SetAlpha()
    {
       Color newColor = spriteRend.material.color;
       newColor.a = (strength / GV.PHEROMONE_MAX_ENERGY) * GV.PHEROMONE_MAX_OPACITY;
       spriteRend.material.color = newColor;
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
            return true;
        }
        return false;
    }
    

}

