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
        strength = 7;
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
            Vector2 homePos= pHome.transform.position;
            Vector2 awayPos = pAway.transform.position;
            Vector2 vecDiff = awayPos - homePos;
            float ang = Mathf.Atan(vecDiff.x / vecDiff.y) * Mathf.Rad2Deg * -1;
            GetComponentInChildren<TextMesh>().text = strength.ToString();
            transform.position = ((awayPos - homePos) / 2) + homePos;
            transform.localScale = new Vector3(1,Vector2.Distance(homePos, awayPos),1);
            transform.eulerAngles = new Vector3(0, 0, ang);
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

