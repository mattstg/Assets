using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PheromoneNode : MonoBehaviour {
    public bool lockedFromDeath = false;
    public bool setToDie = false; //helps with trail merging
    protected List<PheromoneTrail> trails = new List<PheromoneTrail>(); 
    public int pherID = 0;

    public void AddTrail(PheromoneTrail pt)
    {
        if (!trails.Contains(pt))
            trails.Add(pt);
        //serouisly if theres a conflict thatd be weird
    }

    public void InitializeNode(PheromoneTrail toLink)
    {
        if (!trails.Contains(toLink))
            trails.Add(toLink);
    }

    public PheromoneNode AbsorbNode(PheromoneNode toMerge)
    {
        AbsorbTrails(toMerge);
        FindObjectOfType<PheromoneManager>().DeleteNode(toMerge);
        return this;
    }

    public void PheromoneTrailDied(PheromoneTrail pt)
    {
        trails.Remove(pt);
        if (trails.Count <= 0)
            FindObjectOfType<PheromoneManager>().DeleteNode(this);
    }

    public int GetTotalPhermoneWeights(PheromoneTrail excludeTrail)
    {
        int toReturn = 0;
       // Debug.Log("total pher called: " + trails.Count);
        foreach (PheromoneTrail pt in trails)
            if (pt != excludeTrail)
            {
               // Debug.Log("adding: " + pt.strength);
                toReturn += pt.strength;
                //Debug.Log("toReturn: " + toReturn);

            }
        return toReturn;
    }

    private void AbsorbTrails(PheromoneNode toAbsorb)
    {
        //pt.SetNewNode(toMerge, this);
        foreach (PheromoneTrail pt in toAbsorb.trails)
            if (!trails.Contains(pt))
            {
                pt.SetNewNode(toAbsorb, this);
                trails.Add(pt);
            }
    }

    public PheromoneTrail SelectNewTrailByWeight(int goalWeightValue, PheromoneTrail ptToIgnore, int backTrackWeight)
    {
        int currentWeight = 1;
        foreach (PheromoneTrail pt in trails)
        {
            if (pt == ptToIgnore)
                currentWeight += backTrackWeight;
            else
                currentWeight += pt.strength;
            if (goalWeightValue <= currentWeight)
                    return pt;
        }
        return null; //If it goes beyond the size, then it was because SCOUT was chosen
    }

    public void CleanUpBadTrails()
    {
        for (int i = trails.Count - 1; i >= 0; --i)
        {
            trails[i].ValidateTrail(); //will delete and clean them
        }
    }

       
}
