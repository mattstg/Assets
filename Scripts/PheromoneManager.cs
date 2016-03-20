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
        parentNode.initialRoot = false;
        newNode.InitializeNode(newTrail);
        pheromoneNodes.Add(newNode);
        pheromoneTrails.Add(newTrail);
        return newNode;
    }

    private PheromoneNode RetrieveNewNode(GV.PhermoneTypes pherType, Vector2 spawnLocation)
    {
        GameObject newNodeGO = Instantiate(phermoneNodePrefab, spawnLocation, Quaternion.identity) as GameObject;
        PheromoneNode newNode = newNodeGO.GetComponent<PheromoneNode>();
        newNode.initialRoot = true;
        pheromoneNodes.Add(newNode);
        return newNode;
    }

    public static PheromoneNode DropPheromone(PheromoneNode parentNode, GV.PhermoneTypes pherType, Vector2 atPos)
    {
        List<PheromoneNode> allNearbyPher = GetAllNearbyByTag<PheromoneNode>("Node", GV.PHEROMONE_PLACEMENT_ABSORPTION_RADIUS,atPos);
        if (allNearbyPher.Contains(parentNode) && !parentNode.immortal) //this means parent node will be consumed, so it cannot be a parent
        {
            parentNode = null;
        }
        PheromoneNode pn = FindObjectOfType<PheromoneManager>().CreateNewNode(parentNode, pherType, atPos);        
        foreach (PheromoneNode toMerge in allNearbyPher)
        {
            if(!toMerge.immortal && !toMerge.initialRoot)
                pn.AbsorbNode(toMerge);
        }
        return pn;
    }

    public void CreatePheromoneTrail(PheromoneNode home, PheromoneNode away, GV.PhermoneTypes pherType)
    {
        PheromoneTrail newPt = Instantiate<GameObject>(phermoneTrailPrefab).GetComponent<PheromoneTrail>();
        newPt.Initialize(home, away, pherType);
        pheromoneTrails.Add(newPt);
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
        foreach (PheromoneTrail pt in pheromoneTrails)
            pt.SetAlpha();
        
    }
    public List<Intersection> GetIntersections(Vector2 antLocation, Vector2 antDirection)
    {
        float ant_y = antLocation.y;
        float ant_x = antLocation.x;
        GV.directionQuadrants dquad = GV.directionQuadrants.q1;
        if      (antDirection.y >= ant_y && antDirection.x >= ant_x) dquad = GV.directionQuadrants.q1;
        else if (antDirection.y >= ant_y && antDirection.x < ant_x) dquad = GV.directionQuadrants.q2;
        else if (antDirection.y < ant_y && antDirection.x < ant_x) dquad = GV.directionQuadrants.q3;
        else if (antDirection.y < ant_y && antDirection.x >= ant_x) dquad = GV.directionQuadrants.q4;
        

        List<PheromoneNode> relevantNodes = new List<PheromoneNode>();
        List<PheromoneTrail> relevantTrails = new List<PheromoneTrail>();
        List<PheromoneTrail> crossingTrails = new List<PheromoneTrail>();
        List<Intersection> intersectionPoints = new List<Intersection>();

        if ((antDirection.x - ant_x) != 0)
        {
            float slope = (antDirection.y - ant_y) / (antDirection.x - ant_x);
            float y_intercept = ant_y - slope * ant_x;

            switch (dquad)
            {
                case GV.directionQuadrants.q1:
                    foreach (PheromoneNode node in pheromoneNodes)
                    {
                        if (node.transform.position.y >= ant_y && node.transform.position.x >= ant_x)
                        {
                            relevantNodes.Add(node);
                        }
                    }
                    break;
                case GV.directionQuadrants.q2:
                    foreach (PheromoneNode node in pheromoneNodes)
                    {
                        if (node.transform.position.y >= ant_y && node.transform.position.x < ant_x)
                        {
                            relevantNodes.Add(node);
                        }
                    }
                    break;
                case GV.directionQuadrants.q3:
                    foreach (PheromoneNode node in pheromoneNodes)
                    {
                        if (node.transform.position.y < ant_y && node.transform.position.x < ant_x)
                        {
                            relevantNodes.Add(node);
                        }
                    }
                    break;
                case GV.directionQuadrants.q4:
                    foreach (PheromoneNode node in pheromoneNodes)
                    {
                        if (node.transform.position.y < ant_y && node.transform.position.x >= ant_x)
                        {
                            relevantNodes.Add(node);
                        }
                    }
                    break;
                default:
                    break;

            }
            foreach (PheromoneTrail trail in pheromoneTrails)
            {
                if (trail.pHome == null || trail.pAway == null)
                    continue;
                if (relevantNodes.Contains(trail.pHome) || relevantNodes.Contains(trail.pAway))
                    relevantTrails.Add(trail);
            }

            
            foreach (PheromoneTrail trail in relevantTrails)
            {
                if (trail.pHome == null || trail.pAway == null)
                    continue;
                    if ((trail.pHome.transform.position.y >= (slope * trail.pHome.transform.position.x) + y_intercept
                        && trail.pAway.transform.position.y <= (slope * trail.pAway.transform.position.x) + y_intercept)
                        || (trail.pHome.transform.position.y <= (slope * trail.pHome.transform.position.x) + y_intercept
                        && trail.pAway.transform.position.y >= (slope * trail.pAway.transform.position.x) + y_intercept))
                    {
                        crossingTrails.Add(trail);
                    }
            }
            

            foreach (PheromoneTrail trail in pheromoneTrails)
            {
                if (trail.pHome == null || trail.pAway == null)
                    continue;
                float pHome_y = trail.pHome.transform.position.y;
                float pHome_x = trail.pHome.transform.position.x;
                float pAway_y = trail.pAway.transform.position.y;
                float pAway_x = trail.pAway.transform.position.x;

                if ((dquad == GV.directionQuadrants.q1 || dquad == GV.directionQuadrants.q3)
                 && (((pHome_y >= ant_y && pHome_x <= ant_x) && (pAway_y <= ant_y && pAway_x >= ant_x))
                 || ((pHome_y <= ant_y && pHome_x >= ant_x) && (pAway_y >= ant_y && pAway_x <= ant_x))))
                {
                    crossingTrails.Add(trail);
                }

                if ((dquad == GV.directionQuadrants.q2 || dquad == GV.directionQuadrants.q4)
                 && (((pHome_y >= ant_y && pHome_x >= ant_x) && (pAway_y <= ant_y && pAway_x <= ant_x))
                 || ((pHome_y <= ant_y && pHome_x <= ant_x) && (pAway_y >= ant_y && pAway_x >= ant_x))))
                {
                    crossingTrails.Add(trail);
                }
            }

            foreach (PheromoneTrail trail in crossingTrails)
            {
                if (trail.pHome == null || trail.pAway == null)
                    continue;
                Intersection intersectPoint = new Intersection(Vector2.zero, trail);
                float pHome_y = trail.pHome.transform.position.y;
                float pHome_x = trail.pHome.transform.position.x;
                float pAway_y = trail.pAway.transform.position.y;
                float pAway_x = trail.pAway.transform.position.x;
                float trailSlope;
                float trailIntercept;

                if (pHome_x > pAway_x)
                {
                    trailSlope = (pHome_y - pAway_y) / (pHome_x - pAway_x);
                }
                else
                {
                    trailSlope = (pAway_y - pHome_y) / (pAway_x - pHome_x);
                }
                trailIntercept = pHome_y - (trailSlope * pHome_x);
                intersectPoint._intersectionPoint.x = (trailIntercept - y_intercept) / (slope - trailSlope);
                intersectPoint._intersectionPoint.y = slope * intersectPoint._intersectionPoint.x + y_intercept;
                if (((dquad == GV.directionQuadrants.q1 || dquad == GV.directionQuadrants.q4) && (intersectPoint._intersectionPoint.x > ant_x))
                || ((dquad == GV.directionQuadrants.q2 || dquad == GV.directionQuadrants.q3) && (intersectPoint._intersectionPoint.x < ant_x)))
                {
                    intersectionPoints.Add(intersectPoint);

                }
            }

        }
        else
        {
            foreach (PheromoneTrail trail in relevantTrails)
            {
                if (trail.pHome == null || trail.pAway == null)
                    continue;
                if ((trail.pHome.transform.position.x >= ant_x
                    && trail.pAway.transform.position.x <= ant_x)
                    || (trail.pHome.transform.position.x <= ant_x
                    && trail.pAway.transform.position.x >= ant_x))

                {
                    crossingTrails.Add(trail);
                }
            }

            foreach (PheromoneTrail trail in pheromoneTrails)
            {
                if (trail.pHome == null || trail.pAway == null)
                    continue;
                float pHome_y = trail.pHome.transform.position.y;
                float pHome_x = trail.pHome.transform.position.x;
                float pAway_y = trail.pAway.transform.position.y;
                float pAway_x = trail.pAway.transform.position.x;

                if ((dquad == GV.directionQuadrants.q1 || dquad == GV.directionQuadrants.q3)
                 && (((pHome_y >= ant_y && pHome_x <= ant_x) && (pAway_y <= ant_y && pAway_x >= ant_x))
                 || ((pHome_y <= ant_y && pHome_x >= ant_x) && (pAway_y >= ant_y && pAway_x <= ant_x))))
                {
                    crossingTrails.Add(trail);
                }

                if ((dquad == GV.directionQuadrants.q2 || dquad == GV.directionQuadrants.q4)
                 && (((pHome_y >= ant_y && pHome_x >= ant_x) && (pAway_y <= ant_y && pAway_x <= ant_x))
                 || ((pHome_y <= ant_y && pHome_x <= ant_x) && (pAway_y >= ant_y && pAway_x >= ant_x))))
                {
                    crossingTrails.Add(trail);
                }
            }

            foreach (PheromoneTrail trail in crossingTrails)
            {
                if (trail.pHome == null || trail.pAway == null)
                    continue;
                Intersection intersectPoint = new Intersection(Vector2.zero, trail);
                float pHome_y = trail.pHome.transform.position.y;
                float pHome_x = trail.pHome.transform.position.x;
                float pAway_y = trail.pAway.transform.position.y;
                float pAway_x = trail.pAway.transform.position.x;
                float trailSlope;
                float trailIntercept;

                if (pHome_x > pAway_x)
                {
                    trailSlope = (pHome_y - pAway_y) / (pHome_x - pAway_x);
                }
                else
                {
                    trailSlope = (pAway_y - pHome_y) / (pAway_x - pHome_x);
                }
                trailIntercept = pHome_y - (trailSlope * pHome_x);
                intersectPoint._intersectionPoint.x = ant_x;
                intersectPoint._intersectionPoint.y = trailSlope * ant_x + trailIntercept;
                if (((dquad == GV.directionQuadrants.q1 || dquad == GV.directionQuadrants.q4) && (intersectPoint._intersectionPoint.x > ant_x))
                || ((dquad == GV.directionQuadrants.q2 || dquad == GV.directionQuadrants.q3) && (intersectPoint._intersectionPoint.x < ant_x)))
                {
                    intersectionPoints.Add(intersectPoint);

                }
            }
        }

       
        return intersectionPoints;
    }
}
