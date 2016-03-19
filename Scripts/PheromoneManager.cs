using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PheromoneManager : MonoBehaviour
{
    public GameObject phermoneNodePrefab;
    public GameObject phermoneTrailPrefab;
    public static int pherCreationCount;
    List<PheromoneNode> pheromoneNodes = new List<PheromoneNode>();
    List<PheromoneTrail> pheromoneTrails = new List<PheromoneTrail>();
    float updateCounter = 0;

    public PheromoneNode CreateNewNode(PheromoneNode parentNode,GV.PhermoneTypes pherType, Vector2 spawnLocation)
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

    public static PheromoneNode DropPheromone(PheromoneNode parentNode, GV.PhermoneTypes pherType, Vector2 atPos)
    {
        List<PheromoneNode> allNearbyPher = GetAllNearbyByTag<PheromoneNode>("Node", GV.PHEROMONE_PLACEMENT_ABSORPTION_RADIUS,atPos);
        if (allNearbyPher.Contains(parentNode)) //this means parent node will be consumed, so it cannot be a parent
            parentNode = null;        
        PheromoneNode pn = FindObjectOfType<PheromoneManager>().CreateNewNode(parentNode, pherType, atPos);        
        foreach (PheromoneNode toMerge in allNearbyPher)
        {
            pn.AbsorbNode(toMerge);
        }
        //pn.CleanUpBadTrails();  didnt work
        return pn;
    }

    public void CreatePheromoneTrail(PheromoneNode home, PheromoneNode away, GV.PhermoneTypes pherType)
    {
        PheromoneTrail newPt = Instantiate<GameObject>(phermoneTrailPrefab).GetComponent<PheromoneTrail>();
        newPt.Initialize(home, away, pherType);
    }

    public static List<T> GetAllNearbyByTag<T>(string _tag, float searchRadius, Vector2 atPos)
    {
        List<T> toReturn = new List<T>();
        Collider2D[] colis = Physics2D.OverlapCircleAll(atPos, searchRadius);
        foreach (Collider2D coli in colis)
        {
            if (coli.CompareTag(_tag))
                toReturn.Add(coli.gameObject.GetComponent<T>());
        }
        return toReturn;
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
