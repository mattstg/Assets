using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Ant : MonoBehaviour {
    enum AntMode { Forage, Wander, Scout };
	private GameObject holdingSprite;

    AntMode antMode = AntMode.Wander;
    float energy = GV.ANT_ENERGY_START;
    public int scoutingWeight;
    public int backtrackWeight;
    PheromoneTrail currentTrail;
    PheromoneNode lastVisitedNode;
    PheromoneNode goalNode;
    PheromoneNode carryingPheromone;
	public resourceObject holding;
    Vector2 goalSpot = new Vector2(0,0);
    float timeSinceLastEvent = 0;

	public bool wantsToEnterHive =  false;

	// Use this for initialization
    public void Initialize()
    {
        scoutingWeight = 50;
        backtrackWeight = 1;
    }

	void Start () {
        Initialize();
        StartWanderMode();
	}

	void LateUpdate(){
		if (energy <= 0)
			dies ();
	}

	// Update is called once per frame
	void Update () {
        float dTime = Time.deltaTime;
		drainEnergy(GV.ANT_ENERGY_DECAY * dTime);
        
        switch (antMode)
        {
            case AntMode.Forage:
                break;
            case AntMode.Scout:
                ScoutUpdate(dTime);
                break;
            case AntMode.Wander:
                WanderTimeUpdate(dTime);
                break;
        }
        MoveTowardsGoal(dTime);
	}

    
    //
    void StartWanderMode()
    {
        antMode = AntMode.Wander;
        goalSpot = GetRandomGoalVector();
    }

    void WanderTimeUpdate(float dtime)
    {
        timeSinceLastEvent += dtime;
        if (timeSinceLastEvent > GV.ANT_STATE_TIMER)
        {
            timeSinceLastEvent = 0;
            goalSpot = GetRandomGoalVector();
        }
    }

    void MoveTowardsGoal(float dtime)
    {
		Vector2 headingDirection = GV.SubtractVectors (goalSpot, transform.position);
		/* if (Mathf.Abs(headingDirection.x) < 0.1f && Mathf.Abs(headingDirection.y) < 0.1f)
			headingDirection = Vector2.zero;
		else{
			headingDirection.Normalize ();
		} */
		headingDirection.Normalize ();
      	GetComponent<Rigidbody2D>().velocity = headingDirection * GV.ANT_SPEED;

		if (headingDirection != Vector2.zero || headingDirection.y != 0) {
			float angle = -Mathf.Atan (headingDirection.x / headingDirection.y) * Mathf.Rad2Deg;
			if (headingDirection.y < 0f)
				angle += 180; 
			transform.rotation = Quaternion.Euler (0.0f, 0.0f, angle); 
		} else {
			//transform.rotation = Quaternion.Euler (0.0f, 0.0f, angle);
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
        int totalWeight = pn.GetTotalPhermoneWeights(currentTrail) + scoutingWeight + workingBackTrack;
        int randomResult = Random.Range(1, totalWeight + 1);
        currentTrail = pn.SelectNewTrailByWeight(randomResult, currentTrail, workingBackTrack);
        lastVisitedNode = pn;
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
        goalSpot = GetRandomGoalVector();
        timeSinceLastEvent = 0;
        antMode = AntMode.Scout;
        List<Intersection> intersections = GameObject.FindObjectOfType<PheromoneManager>().GetIntersections(transform.position, goalSpot);
        foreach (Intersection _intersection in intersections)
        {
            Instantiate(Resources.Load("prefab/FoodResourcePrefab"), _intersection._intersectionPoint, Quaternion.identity);
        }

    }

    

    Vector2 GetRandomGoalVector()
    {
        return GV.AddVectors(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * (GV.ANT_STATE_TIMER + 1) * GV.ANT_SPEED, transform.position);
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
        if (coliNode  && coliNode != lastVisitedNode)
        {
             if (antMode == AntMode.Scout)
             {
                 PheromoneManager.DropPheromone(lastVisitedNode, GetPherType(), transform.position);
             }
             ArriveAtNode(coliNode);
		}else if(coli.gameObject.GetComponent<DeadAntScript>() != null){
			//Just touuched dead ant
			takeDamage(GV.DMG_FROM_ANT_CORPSES * Time.deltaTime);
		}
        else if(coli.CompareTag("Ant"))
        {

        }
    }
    
	public void eatResource(float quantToEat){
		resourceObject temp;
		if (holding.quantity > quantToEat) {
			holding.quantity -= quantToEat;
			temp = new resourceObject (holding);
			temp.quantity = quantToEat; 
		} else {
			temp = new resourceObject (holding.give ());
			refreshHoldingResource ();
		}

		if (holding.isPoison) {
			takeDamage(temp.quantity * GV.POISON_TO_ENRGY_HP);
		} else {
			addEnergy(temp.quantity * GV.RESOURCE_TO_ENRGY_HP);
		}
	}

	/*
	public void regenHealth(float healing){
		health += healing;
	} */

	public float drainEnergy(float energyOut){
		energy -= energyOut;
		return energyOut;
	}

	public void addEnergy(float energyIn){
		energy += energyIn;
	}

	public float retEnergy(){
		return energy;
	}

	public void setEnergy(float toSet){
		energy = toSet;
	}
		
	public void takeDamage(float dmgIn){
		energy -= dmgIn;
	}

	public resourceObject giveResource(){
		//pass the thing to ANT or colony
		resourceObject temp = new resourceObject(holding.give());
		refreshHoldingResource ();
		return temp;
	}

	public resourceObject giveResource(float quantToGive){
		if (holding.quantity < quantToGive)
			return giveResource ();
		holding.quantity -= quantToGive;
		return new resourceObject (holding.resType, quantToGive, holding.isPoison);
	}

	public resourceObject whatResourceHolding(){
		return holding;
	}

	public void takeResource(resourceObject resourceToHold){
		//Debug.Log ("Taking Resource;");
		holding = new resourceObject(resourceToHold);
		refreshHoldingResource ();
	}

	private void dies(){
		 //(Resources.Load ("Sprites/DeadAnt"));
		//gameObject.GetComponent<SpriteRenderer>().sprite = null;// = Resources.Load ("Sprites/DeadAnt") as Sprite;
		GetComponent<SpriteRenderer>().sprite = Instantiate(Resources.Load<Sprite>("Sprites/DeadAnt"));
		gameObject.AddComponent<DeadAntScript> ();
		GetComponent<Rigidbody2D> ().angularVelocity = 0f;
		GetComponent<Rigidbody2D> ().velocity = Vector3.zero;
		GetComponent<Rigidbody2D> ().drag = 0.5f;
		FindObjectOfType<colonyScript> ().totalNumberOfAnts--;
		Destroy (this);
	}

    void ScoutUpdate(float dtime)
    {
        timeSinceLastEvent += dtime;
        if (timeSinceLastEvent > GV.ANT_STATE_TIMER)
        {
            DropPheromone();
        }
    }

    PheromoneNode DropPheromone()
    {
        PheromoneNode pn;
        pn = PheromoneManager.DropPheromone(lastVisitedNode, GetPherType(), transform.position);
        lastVisitedNode = pn;
        timeSinceLastEvent = 0;
        ArriveAtNode(pn);
        return pn;
    }

    void DropPheromoneOnExistingTrail(PheromoneTrail pt)
    {
        pt.SplitByNode(DropPheromone());
        Debug.Log("drop phermone on existing trail");
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

	private void refreshHoldingResource(){
		//if (holding != null) {
			if (!holding.isZero ()) {
				string prefabName = "WaterResourcePrefab";
				if (holding.resType == GV.ResourceTypes.Food)
					prefabName = "FoodResourcePrefab";
				holdingSprite = Instantiate (Resources.Load ("Prefab/" + prefabName)) as GameObject;
			holdingSprite.transform.parent = gameObject.transform;
			holdingSprite.transform.localPosition = new Vector3(0f, 0.3f, 0f);
				//holdingSprite.transform.position.Set (0f,0f,0f);
				//holdingSprite.transform.position = new Vector3(0f,0.3f,0f);
					//holdingSprite.AddComponent<transformLock> ().Initialize (holdLoc.transform);
			} else if (holdingSprite != null) {
				Destroy (holdingSprite);
			}
		//}
	}

    private List<T> GetAllNearbyByTag<T>(string _tag, float searchRadius)
    {
        List<T> toReturn = new List<T>();
        Collider2D[] colis = Physics2D.OverlapCircleAll(transform.position, searchRadius);
        foreach (Collider2D coli in colis)
        {
            if (coli.CompareTag(_tag))
                toReturn.Add(coli.gameObject.GetComponent<T>());
        }
        return toReturn;
    }


    public void DEBUG_CordycepControl(Vector2 newGoal)
    {
        Debug.Log("new goal spot is: " + newGoal);
        goalSpot = newGoal;
        timeSinceLastEvent = 0;
        antMode = AntMode.Scout;
       
    }
}

