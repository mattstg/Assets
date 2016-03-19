using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PheromoneNode : MonoBehaviour {
    public bool lockedFromDeath = false;
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
        foreach (PheromoneTrail pt in trails)
            if (pt != excludeTrail)
                toReturn += pt.strength;
        return toReturn;
    }

    private void AbsorbTrails(PheromoneNode toAbsorb)
    {
        foreach (PheromoneTrail pt in toAbsorb.trails) //for each trail i will absorb
        {
            
           Debug.Log("tails to destroy");
           pt.SetNewNode(toAbsorb, this);             //setup
            
           PheromoneTrail trailToAbsorbTrail = PheromoneTrail.PherListContains(trails, pt);
           if (!trailToAbsorbTrail)
           {
               Debug.Log("TurtlesAreNigh");
               trails.Add(pt);
           }
           else
           {
               Debug.Log("combine power");
               trailToAbsorbTrail.strength += pt.strength;
               pt.SetNewNode(null, null);   //since it already exist, and we dont need it, delete it.
           }
        }
           
    }

    public PheromoneTrail SelectNewTrailByWeight(int goalWeightValue, PheromoneTrail ptToIgnore, int backTrackWeight)
    {

        Debug.Log("starts");
        int currentWeight = 1;
        foreach (PheromoneTrail pt in trails)
        {
            if (pt == ptToIgnore)
                currentWeight += backTrackWeight;
            else
                currentWeight += pt.strength;
            if (goalWeightValue <= currentWeight)
            {
                Debug.Log("ends");
                return pt;
            }
        }
        Debug.Log("ends null");
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
