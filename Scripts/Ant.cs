﻿using UnityEngine;
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
        if (currentTrail)
        {
            Vector2 headingDirection = GV.SubtractVectors(goalSpot, transform.position).normalized;
            GetComponent<Rigidbody2D>().velocity = headingDirection * GV.ANT_SPEED;
        }
        else  //trail died, become wanderer
        {
            WanderMode();
        }
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
        Debug.Log("out: " + pn.GetTotalPhermoneWeights(currentTrail));
        int totalWeight = pn.GetTotalPhermoneWeights(currentTrail) + scoutingWeight + workingBackTrack;
        int randomResult = Random.Range(1, totalWeight + 1);
        Debug.Log("Result: " + randomResult + "/" + totalWeight);
        currentTrail = pn.SelectNewTrailByWeight(randomResult, currentTrail, workingBackTrack);
        if (currentTrail)
        {
            goalSpot = currentTrail.GetOtherNode(pn).transform.position;
        }
        else
            ScoutModeActivate();
    }

    void ScoutModeActivate()
    {
        goalSpot = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)).normalized * GV.ANT_SCOUT_TIMER * GV.ANT_SPEED;
        timeSinceLastNode = 0;
        antMode = AntMode.Scout;
        carryingPheromone = FindObjectOfType<PheromoneManager>().RetrieveNewNode(lastVisitedNode, GV.PhermoneTypes.Friendly);
        carryingPheromone.gameObject.AddComponent<transformLock>().Initialize(transform);
        carryingPheromone.carried = true;
        //Needs to create a node here
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
        if (coli.CompareTag("Node") && coli.GetComponent<PheromoneNode>() != lastVisitedNode)
        {
            if(!coli.GetComponent<PheromoneNode>().carried)
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

    void ScoutUpdate(float dtime)
    {
        timeSinceLastNode += dtime;
        if (timeSinceLastNode > GV.ANT_SCOUT_TIMER)
        {
            Debug.Log("DROPS A PHER");
            timeSinceLastNode = 0;
        }
    }
}
