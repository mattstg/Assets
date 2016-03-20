using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Ant : MonoBehaviour {
    public enum AntMode { Forage, Wander, Scout };
	private GameObject holdingSprite;

    public AntMode antMode = AntMode.Wander;
    float energy = GV.ANT_ENERGY_MAX;
    public int scoutingWeight;
    public int backtrackWeight;
    PheromoneTrail currentTrail;
    PheromoneNode lastVisitedNode;
    PheromoneNode goalNode;
    PheromoneNode carryingPheromone;
	public resourceObject holding;
    Vector2 goalSpot = new Vector2(0,0);
    float timeSinceLastEvent = 0;
    Intersection closestIntersection;
    

	public bool wantsToEnterHive =  false;
	public bool hasBackTracked = false;

	// Use this for initialization
    public void Initialize(int _scoutingWeight = 1)
    {
        scoutingWeight = _scoutingWeight;
        backtrackWeight = 1;
    }

	void Start () {
        //Initialize();
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
        if ((int)transform.position.x == (int)goalSpot.x && (int)transform.position.y == (int)goalSpot.y)
            FindNodeUnderAnt();            //so you've reached your goal, what next
        
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

    void FindNodeUnderAnt()
    {
        List<PheromoneNode> pherSetting = PheromoneManager.GetAllNearbyByTag<PheromoneNode>("Node", 1, transform.position);
        if (pherSetting.Count >= 1)
            ArriveAtNode(pherSetting[0]);
        else
            ScoutModeActivate();
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
		headingDirection.Normalize ();
      	GetComponent<Rigidbody2D>().velocity = headingDirection * GV.ANT_SPEED;
		float angle = 0;
		if (headingDirection.y < 0f)
			 angle = 180; 
		if(headingDirection.y != 0)
			angle = angle +-Mathf.Atan (headingDirection.x / headingDirection.y) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler (0.0f, 0.0f, angle); 
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
		PheromoneTrail isBackTrackTrail = currentTrail;
		if (hasBackTracked) {
			PheromoneTrail tempTrail = pn.SelectNewTrailByWeight(randomResult, currentTrail, workingBackTrack);
			currentTrail = pn.SelectNewTrailByWeight(randomResult, currentTrail, workingBackTrack);
			if(currentTrail == null || currentTrail.strength > tempTrail.strength)
				currentTrail = tempTrail;
			hasBackTracked = false;
		} else {
			currentTrail = pn.SelectNewTrailByWeight(randomResult, currentTrail, workingBackTrack);
		}
		if (currentTrail != null && isBackTrackTrail != null && isBackTrackTrail == currentTrail) {
			hasBackTracked = !hasBackTracked;
		}
        lastVisitedNode = pn;
        if (currentTrail)
        {
            if (currentTrail.GetOtherNode(pn) == null)
                currentTrail.TrailDie();
            else
            {
                goalSpot = currentTrail.GetOtherNode(pn).transform.position;
                antMode = AntMode.Forage;
            }
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
        if (intersections.Count >= 1)
        {
            closestIntersection = intersections[0];
            for (int i = 1; i < intersections.Count; ++i)
            {
                if (closestIntersection._intersectionPoint.magnitude > intersections[i]._intersectionPoint.magnitude) //bit proccessor intensive, but saves on total updates
                    closestIntersection = intersections[i];
            }
            Debug.Log("values found");
            Instantiate(Resources.Load("Prefab/FoodResourcePrefab"), closestIntersection._intersectionPoint, Quaternion.identity);
        }
        else
        {
            closestIntersection = null;
        }
    }



    Vector2 GetRandomGoalVector()
    {
        float randAng = Random.Range(0,360)*Mathf.Deg2Rad;
        return GV.AddVectors(new Vector2((float)Mathf.Cos(randAng), (float)Mathf.Sin(randAng)) * (GV.ANT_STATE_TIMER + 1) * GV.ANT_SPEED, transform.position);
        //return GV.AddVectors(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * (GV.ANT_STATE_TIMER + 1) * GV.ANT_SPEED, transform.position);
    }


    void WanderMode()
    {

    }

    void OnTriggerEnter2D(Collider2D coli)
    {
        PheromoneNode coliNode = coli.GetComponent<PheromoneNode>();
        if (coliNode  && coliNode != lastVisitedNode)
        {
             if (antMode == AntMode.Scout && coliNode.trails.Count >= 1)
             {
                 DropPheromone();
                 //PheromoneManager.DropPheromone(lastVisitedNode, GetPherType(), transform.position);
             }
             ArriveAtNode(coliNode);
		}else if(coli.gameObject.GetComponent<DeadAntScript>() != null){
			//Just touuched dead ant
			takeDamage(GV.DMG_FROM_CORPSES * Time.deltaTime);
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
			takeDamage(temp.quantity * GV.POISON_TO_ENRGY);
		} else {
			addEnergy(temp.quantity * GV.RESOURCE_TO_ENRGY);
		}
	}

	/*
	public void regenHealth(float healing){
		health += healing;
	} */

	public float drainEnergy(float energyOut){
		energy -= energyOut;
		if(energy < GV.PRCNT_ENRGY_THRESHHOLD * GV.ANT_ENERGY_MAX)
			wantsToEnterHive = true;
		return energyOut;
	}

	public void addEnergy(float energyIn){
		energy += energyIn;
		if(energy > GV.PRCNT_ENRGY_THRESHHOLD * GV.ANT_ENERGY_MAX && holding.isZero())
			wantsToEnterHive = false;
	}

	public float retEnergy(){
		return energy;
	}

	public void setEnergy(float toSet){
		energy = toSet;
	}
		
	public void takeDamage(float dmgIn){
		energy -= dmgIn;
		if(energy < GV.PRCNT_ENRGY_THRESHHOLD * GV.ANT_ENERGY_MAX)
			wantsToEnterHive = true;
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
        if(closestIntersection != null)
            if ((int)closestIntersection._intersectionPoint.x == (int)transform.position.x && (int)closestIntersection._intersectionPoint.y == (int)transform.position.y)
            {
                DropPheromoneOnExistingTrail(closestIntersection._intersectionTrail);
            }
    }

    PheromoneNode DropPheromone()
    {
        PheromoneNode pn = PheromoneManager.DropPheromone(lastVisitedNode, GetPherType(), transform.position);
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
		if (!holding.isZero ()) {
			string prefabName = "WaterResourcePrefab";
			if (holding.resType == GV.ResourceTypes.Food)
				prefabName = "FoodResourcePrefab";
			holdingSprite = Instantiate (Resources.Load ("Prefab/" + prefabName)) as GameObject;
			holdingSprite.transform.parent = gameObject.transform;
			holdingSprite.transform.localPosition = new Vector3(0f, 0.3f, 0f);
			wantsToEnterHive = true;
		} else if (holdingSprite != null) {
			Destroy (holdingSprite);
			wantsToEnterHive = false;
		} else if (holding.isZero())
			wantsToEnterHive = false;
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
        goalSpot = newGoal;
        timeSinceLastEvent = 0;
        antMode = AntMode.Scout;
    }
}

