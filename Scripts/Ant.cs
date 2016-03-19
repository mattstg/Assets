using UnityEngine;
using System.Collections;

public class Ant : MonoBehaviour {

    float energy = GV.ANT_ENERGY_START;
    public int scoutingWeight;
    PheromoneTrail currentTrail;
    PheromoneNode lastVisitedNode;
    Vector2 goalSpot = new Vector2(0,0);
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
        MoveTowardsNode(Time.deltaTime);
	}

    void MoveTowardsNode(float dtime)
    {
        Vector2 headingDirection = GV.SubtractVectors(goalSpot, transform.position).normalized;
        GetComponent<Rigidbody2D>().velocity = headingDirection * GV.ANT_SPEED;
    }

    void ArriveAtNode(PheromoneNode pn)
    {
        int totalWeight = pn.GetTotalPhermoneWeights(currentTrail) + scoutingWeight;
        int randomResult = Random.Range(1, totalWeight + 1);
        currentTrail = pn.SelectNewTrailByWeight(randomResult, currentTrail);
        if (currentTrail)
        {
            goalSpot = currentTrail.GetOtherNode(pn).transform.position;
        }
        else
            ScoutModeActivate();
    }

    void ScoutModeActivate()
    {
        goalSpot = new Vector2(10, 10);
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
