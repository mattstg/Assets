using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Ant : MonoBehaviour {
    enum AntMode { Forage, Wander, Scout };
	public GameObject holdLoc;
	private GameObject holdingSprite;

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

	public bool wantsToEnterHive =  false;

	// Use this for initialization
    public void Initialize()
    {
        scoutingWeight = 20;
        backtrackWeight = 1;
    }

	void Start () {
        Initialize();
	}

	void LateUpdate(){
		//dies ();
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

        //drawing animation
       var angle = Mathf.Atan(headingDirection.x / headingDirection.y) * Mathf.Rad2Deg;
       Debug.Log("angle is: " + angle);
       transform.rotation = Quaternion.Euler(0.0f, 0.0f, -angle); 
       //Debug.Log("should be facing: " + headingDirection);
       //transform.rotation = Quaternion.FromToRotation(new Vector3(0, 0, transform.rotation.eulerAngles.z), headingDirection);
       //Debug.Log("is actaully be facing: " + transform.rotation.eulerAngles);
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
        goalSpot = (Quaternion.AngleAxis(Random.Range(0, 360), Vector3.forward) * Vector3.right).normalized * (GV.ANT_SCOUT_TIMER + 1) * GV.ANT_SPEED + transform.position;
        timeSinceLastNode = 0;
        antMode = AntMode.Scout;
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
            Debug.Log("COLISIONS");
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
		gameObject.GetComponent<SpriteRenderer>().sprite = Instantiate(Resources.Load<Sprite>("Sprites/DeadAnt"));
		gameObject.AddComponent<DeadAntScript> ();
		Destroy (this);
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
        PheromoneNode pn;
        pn = PheromoneManager.DropPheromone(lastVisitedNode, GetPherType(), transform.position);
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

}

