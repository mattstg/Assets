using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PheromoneNode : MonoBehaviour {
    public bool immortal = false;
    public bool setToDie = false; //helps with trail merging
    public List<PheromoneTrail> trails = new List<PheromoneTrail>(); 
    public int pherID = 0;

    public void AddTrail(PheromoneTrail pt)
    {
        if (!trails.Contains(pt))
            trails.Add(pt);
        //serouisly if theres a conflict thatd be weird
    }

    public void InitializeNode(PheromoneTrail toLink)
    {
        if(toLink)
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
        if (trails.Count <= 0 && !immortal)
            FindObjectOfType<PheromoneManager>().DeleteNode(this);
    }

    public int GetTotalPhermoneWeights(PheromoneTrail excludeTrail)
    {
        int toReturn = 0;
        foreach (PheromoneTrail pt in trails)
            if (pt != excludeTrail && pt != null)
                toReturn += pt.strength;
        return toReturn;
    }

    private void AbsorbTrails(PheromoneNode toAbsorb)
    {
        foreach (PheromoneTrail pt in toAbsorb.trails) //for each trail i will absorb
        {
           pt.SetNewNode(toAbsorb, this);             //setup
           PheromoneTrail trailToAbsorbTrail = PheromoneTrail.PherListContains(trails, pt);
           if (!trailToAbsorbTrail)
           {
               trails.Add(pt);
           }
           else
           {
               trailToAbsorbTrail.strength += pt.strength;
               //pt.TrailDie();
               pt.SetNewNode(null, null);   //since it already exist, and we dont need it, delete it.
           }
        }
           
    }

    public PheromoneTrail SelectNewTrailByWeight(int goalWeightValue, PheromoneTrail ptToIgnore, int backTrackWeight)
    {

        int currentWeight = 1;
        foreach (PheromoneTrail pt in trails)
        {
            if (pt == null)
            {
                Debug.Log("so null things are a thing");
            }
            if (pt == ptToIgnore)
                currentWeight += backTrackWeight;
            else
                currentWeight += pt.strength;
            if (goalWeightValue <= currentWeight)
            {
                return pt;
            }
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
