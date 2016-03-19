using UnityEngine;
using System.Collections;

public class Ant : MonoBehaviour {

    float energy = GV.ANT_ENERGY_START;
    public int scoutingWeight;
    PheromoneTrail currentTrail;
    PheromoneNode lastVisitedNode;
    PheromoneNode goalNode;
	private resourceObject holding;

	// Use this for initialization
    public void Initialize()
    {
        scoutingWeight = 1;
    }

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        energy -= GV.ANT_ENERGY_DECAY * Time.deltaTime;
	}

    void ArriveAtNode(PheromoneNode pn)
    {
        int totalWeight = pn.GetTotalPhermoneWeights(currentTrail) + scoutingWeight;
        int randomResult = Random.Range(1, totalWeight + 1);
        currentTrail = pn.SelectNewTrailByWeight(randomResult, currentTrail);
        if (currentTrail)
            FollowNewPher();
        else
            ScoutModeActivate();
    }

    void ScoutModeActivate()
    {
        Debug.Log("Scout mode activated");
    }

    void FollowNewPher()
    {
        Debug.Log("follow new pher");
    }

    void OnTriggerEnter2D(Collider2D coli)
    {
        if (coli.CompareTag("Node") && coli.GetComponent<PheromoneNode>() != lastVisitedNode)
        {
            //then at goal or a new node
            ArriveAtNode(coli.GetComponent<PheromoneNode>());
        }

    }
    
	public resourceObject giveResource(){
		//pass the thing to ANT or colony
		return holding.give();
	}

	public resourceObject whatResourceHolding(){
		return holding;
	}

	public void takeResource(resourceObject resourceToHold){
		holding = resourceToHold;
	}
}
