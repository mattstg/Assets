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

}

