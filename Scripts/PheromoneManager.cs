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

    public PheromoneNode RetrieveNewNode(PheromoneNode parentNode,GV.PhermoneTypes pherType, Vector2 spawnLocation)
    {
        if (parentNode == null)
            return RetrieveNewNode(pherType,spawnLocation);
        GameObject newNodeGO = Instantiate(phermoneNodePrefab, spawnLocation, Quaternion.identity) as GameObject;
        GameObject newTrailGO = Instantiate<GameObject>(phermoneTrailPrefab);
        PheromoneNode newNode = newNodeGO.GetComponent<PheromoneNode>();
        PheromoneTrail newTrail = newTrailGO.GetComponent<PheromoneTrail>();

        newTrail.Initialize(parentNode, newNode, pherType);
        parentNode.AddTrail(newTrail);
        newNode.InitializeNode(newTrail);
        pheromoneNodes.Add(newNode);
        pheromoneTrails.Add(newTrail);
        return newNode;
    }

    private PheromoneNode RetrieveNewNode(GV.PhermoneTypes pherType, Vector2 spawnLocation)
    {
        GameObject newNodeGO = Instantiate(phermoneNodePrefab, spawnLocation, Quaternion.identity) as GameObject;
        PheromoneNode newNode = newNodeGO.GetComponent<PheromoneNode>();
        pheromoneNodes.Add(newNode);
        return newNode;
    }

    public void CreatePheromoneTrail(PheromoneNode home, PheromoneNode away, GV.PhermoneTypes pherType)
    {
        PheromoneTrail newPt = Instantiate<GameObject>(phermoneTrailPrefab).GetComponent<PheromoneTrail>();
        newPt.Initialize(home, away, pherType);
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
