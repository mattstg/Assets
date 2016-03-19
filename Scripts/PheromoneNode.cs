using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PheromoneNode : MonoBehaviour {
    protected List<PheromoneTrail> trails = new List<PheromoneTrail>(); 

    public void InitializeNode(List<PheromoneTrail> toLink)
    {
        MergeTrails(toLink);
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

    private void MergeTrails(List<PheromoneTrail> toLink)
    {
        foreach (PheromoneTrail pt in toLink)
            if (!trails.Contains(pt))
                trails.Add(pt);
    }
    
}
