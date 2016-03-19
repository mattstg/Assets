using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PheromoneNode : MonoBehaviour {
    List<PheromoneTrail> trails = new List<PheromoneTrail>();

    public void InitializeNode(List<PheromoneTrail> toLink)
    {
        foreach (PheromoneTrail pt in toLink)
            if (!trails.Contains(pt))
                trails.Add(pt);
    }

    public void InitializeNode(PheromoneTrail toLink)
    {
        if (!trails.Contains(toLink))
            trails.Add(toLink);
    }

    public void MergeNode(PheromoneNode toMerge)
    {

    }

    public void DeletePheromoneTrail(PheromoneTrail pt)
    {
        trails.Remove(pt);
    }
    
}
