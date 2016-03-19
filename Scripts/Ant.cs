using UnityEngine;
using System.Collections;

public class Ant : MonoBehaviour {
    enum AntMode { Forage, Wander, Scout };
	public GameObject holdLoc;
	private GameObject holdingSprite;

    AntMode antMode = AntMode.Forage;
    float energy = GV.ANT_ENERGY_START;
	float health = GV.ANT_MAX_HEALTH;
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
		if (energy <= 0)
			health -= GV.DMG_DUE_TO_STARVATION * Time.deltaTime;
		if (health <= 0)
			dies ();
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
			energy -= temp.quantity * GV.POISON_TO_ENRGY_HP;
			takeDamage(temp.quantity * GV.POISON_TO_ENRGY_HP);
		} else {
			energy += temp.quantity * GV.RESOURCE_TO_ENRGY_HP;
			regenHealth(temp.quantity * GV.RESOURCE_TO_ENRGY_HP);
		}
	}

	public void regenHealth(float healing){
		health += healing;
	}

	public float retHealth(){
		return health;
	}

	public void setHealth(float toSet){
		health = toSet;
	}

	public float retEnergy(){
		return energy;
	}

	public void takeDamage(float dmgIn){
		health -= dmgIn;
		//release dmg pharemones
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
		holding = new resourceObject(resourceToHold);
		refreshHoldingResource ();
	}

	private void dies(){
		GetComponent<SpriteRenderer> ().sprite = Resources.Load ("Sprites/DeadAnt") as Sprite;
		gameObject.AddComponent<DeadAntScript> ();
		Destroy (this);
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

	private void refreshHoldingResource(){
		if (holding != null) {
			if (!holding.isZero ()) {
				string prefabName = "WaterResourcePrefab";
				if (holding.resType == GV.ResourceTypes.Food)
					prefabName = "FoodResourcePrefab";
				holdingSprite = Instantiate (Resources.Load ("Prefab/" + prefabName)) as GameObject;
				holdingSprite.AddComponent<transformLock> ().Initialize (holdLoc.transform);
			} else if (holdingSprite != null) {
				Destroy (holdingSprite);
			}
		}
	}

}
