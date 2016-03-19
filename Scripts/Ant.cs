using UnityEngine;
using System.Collections;

public class Ant : MonoBehaviour {
    enum AntMode { Forage, Wander, Scout };

    AntMode antMode = AntMode.Forage;
    float energy = GV.ANT_ENERGY_START;
    public int scoutingWeight;
    public int backtrackWeight;
    PheromoneTrail currentTrail;
    PheromoneNode lastVisitedNode;
    PheromoneNode goalNode;
    PheromoneNode carryingPheromone;
	public resourceObject holding;
    Vector2 goalSpot = new Vector2(0,0);
    float timeSinceLastNode = 0;

	// Use this for initialization
    public void Initialize()
    {
        scoutingWeight = 20;
        backtrackWeight = 1;
    }

	void Start () {
        Initialize();
	}
	
	// Update is called once per frame
	void Update () {
        float dTime = Time.deltaTime;
        energy -= GV.ANT_ENERGY_DECAY * dTime;
        
        switch (antMode)
        {
            case AntMode.Forage:
                MoveTowardsGoal(dTime);
                break;
            case AntMode.Scout:
                ScoutUpdate(dTime);
                MoveTowardsGoal(dTime);
                break;
            case AntMode.Wander:
                break;
        }
	}

    void MoveTowardsGoal(float dtime)
    {
       Vector2 headingDirection = GV.SubtractVectors(goalSpot, transform.position).normalized;
       GetComponent<Rigidbody2D>().velocity = headingDirection * GV.ANT_SPEED;
    }

    void ArriveAtNode(PheromoneNode pn)
    {
        int workingBackTrack = backtrackWeight;
        if (currentTrail) //since might get deleted during
        {
            currentTrail.strength++;
        }
        else
        {
            workingBackTrack = 0;
        }
        //Debug.Log("out: " + pn.GetTotalPhermoneWeights(currentTrail));
        int totalWeight = pn.GetTotalPhermoneWeights(currentTrail) + scoutingWeight + workingBackTrack;
        int randomResult = Random.Range(1, totalWeight + 1);
        //Debug.Log("Result: " + randomResult + "/" + totalWeight);
        currentTrail = pn.SelectNewTrailByWeight(randomResult, currentTrail, workingBackTrack);
        if (currentTrail)
        {
            goalSpot = currentTrail.GetOtherNode(pn).transform.position;
            antMode = AntMode.Forage;
        }
        else
            ScoutModeActivate();
    }

    void ScoutModeActivate()
    {
        goalSpot = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized * (GV.ANT_SCOUT_TIMER+1) * GV.ANT_SPEED;
        timeSinceLastNode = 0;
        antMode = AntMode.Scout;
        Debug.Log("Scout mode activated, spot chosen: " + goalSpot);
    }

    void FollowNewPher()
    {
        Debug.Log("follow new pher");
    }

    void WanderMode()
    {

    }

    void OnTriggerEnter2D(Collider2D coli)
    {
        PheromoneNode coliNode = coli.GetComponent<PheromoneNode>();
        if (coli.CompareTag("Node") && coliNode != lastVisitedNode)
        {
             if (antMode == AntMode.Scout)
             {
                 DropPheromoneOnExistingNode(coliNode);
             }
             ArriveAtNode(coliNode);
        }
        else if(coli.CompareTag("Ant"))
        {

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
		holding = new resourceObject(resourceToHold);
	}

    void ScoutUpdate(float dtime)
    {
        timeSinceLastNode += dtime;
        if (timeSinceLastNode > GV.ANT_SCOUT_TIMER)
        {
            DropPheromone();
        }
    }

    PheromoneNode DropPheromone()
    {
        PheromoneNode pn = FindObjectOfType<PheromoneManager>().RetrieveNewNode(lastVisitedNode, GetPherType(),transform.position);
        lastVisitedNode = pn;
        timeSinceLastNode = 0;
        ArriveAtNode(pn);
        return pn;
    }

    void DropPheromoneOnExistingTrail(PheromoneTrail pt)
    {
        pt.SplitByNode(DropPheromone());
        Debug.Log("drop phermone on existing trail");
    }

    void DropPheromoneOnExistingNode(PheromoneNode pt)
    {
        PheromoneNode newPher = FindObjectOfType<PheromoneManager>().RetrieveNewNode(lastVisitedNode,GetPherType(),transform.position);
        pt.MergeNode(newPher);
        Debug.Log("drop phermone on existing node");
    }

    GV.PhermoneTypes GetPherType()
    {
        switch (antMode)
        {
            case AntMode.Scout:
            case AntMode.Forage:
            case AntMode.Wander:
                return GV.PhermoneTypes.Friendly;
            default:
                return GV.PhermoneTypes.Friendly;
        }
    }
}

