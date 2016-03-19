using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PheromoneNode : MonoBehaviour {
    protected List<PheromoneTrail> trails = new List<PheromoneTrail>(); 

    public void InitializeNode(List<PheromoneTrail> toLink)
    {
        MergeTrails(toLink);
    }

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

    public void MergeNode(PheromoneNode toMerge)
    {
        MergeTrails(toMerge.trails);
        foreach (PheromoneTrail pt in trails)
        {
            pt.SetNewNode(toMerge, this);
        }
        FindObjectOfType<PheromoneManager>().DeleteNode(toMerge);
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
        foreach (PheromoneTrail pt in trails)
            if (pt != excludeTrail)
                toReturn += pt.strength;
        return toReturn;
    }

    private void MergeTrails(List<PheromoneTrail> toLink)
    {
        foreach (PheromoneTrail pt in toLink)
            if (!trails.Contains(pt))
                trails.Add(pt);
    }

    public PheromoneTrail SelectNewTrailByWeight(int goalWeightValue, PheromoneTrail ptToIgnore)
    {
        int currentWeight = 1;
        foreach (PheromoneTrail pt in trails)
        {
            Debug.Log("Current weight: " + currentWeight + ", out of the goal: " + goalWeightValue);
            if (pt != ptToIgnore)
            {
                if (goalWeightValue <= currentWeight)
                    return pt;
                else
                    currentWeight += pt.strength;
            }
        }
        return null; //If it goes beyond the size, then it was because SCOUT was chosen
    }


       
}
