using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PheromoneManager : MonoBehaviour
{
    public GameObject phermoneNodePrefab;
    public GameObject phermoneTrailPrefab;
    List<PheromoneNode> pheromoneNodes = new List<PheromoneNode>();
    List<PheromoneTrail> pheromoneTrails = new List<PheromoneTrail>();
    float updateCounter = 0;

    public PheromoneNode RetrieveNewNode(PheromoneNode parentNode,GV.PhermoneTypes pherType)
    {
        GameObject newNodeGO = Instantiate<GameObject>(phermoneNodePrefab);
        GameObject newTrailGO = Instantiate<GameObject>(phermoneTrailPrefab);
        PheromoneNode newNode = newNodeGO.GetComponent<PheromoneNode>();
        PheromoneTrail newTrail = newTrailGO.GetComponent<PheromoneTrail>();

        newTrail.Initialize(parentNode, newNode, pherType);
        newNode.InitializeNode(newTrail);
        pheromoneNodes.Add(newNode);
        pheromoneTrails.Add(newTrail);
        return newNode;
    }

    public void DeleteNode(PheromoneNode pn)
    {
        pheromoneNodes.Remove(pn);
        GameObject.Destroy(pn.gameObject);
    }

    public void DeleteTrail(PheromoneTrail pt)
    {
        pheromoneTrails.Remove(pt);
        Destroy(pt.gameObject);
    }

    public void Update()
    {
        updateCounter -= Time.deltaTime;
        if (updateCounter <= 0)
        {
            updateCounter = GV.PATH_DECAY_RATE;
            for (int i = pheromoneTrails.Count - 1; i >= 0; --i)
                pheromoneTrails[i].GetUpdated();
        }
    }
}
