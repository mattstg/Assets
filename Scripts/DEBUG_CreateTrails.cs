using UnityEngine;
using System.Collections;

public class DEBUG_CreateTrails : MonoBehaviour {
    public PheromoneManager pherManager;
    public PheromoneNode initialNode;
	// Use this for initialization
	void Start () {
        PheromoneNode pn = pherManager.RetrieveNewNode(initialNode, GV.PhermoneTypes.Friendly);
        Vector2 randomSpot = new Vector2(Random.Range(-5,5),Random.Range(-5,5));
        pn.transform.position = randomSpot;
	}
	
}
