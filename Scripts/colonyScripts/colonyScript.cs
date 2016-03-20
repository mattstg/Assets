using UnityEngine;
using System.Collections;

public class colonyScript : MonoBehaviour {

	//antHill
	antHillScript antHillLink;

	//Vars
	public int totalNumberOfAnts;
	private int numberOfDormantAnts;
	private float foodStored;
	private float waterStored;
	private bool antResourceDrainTracker = false;
	private float averageAntErgy = 100f;
    private float totalAntsExited = 0;
	private float antsBornThisSecond = 0;
	private int antsExitedThisSecond = 0;
	private float lastSecond = 0;
	private float lastSecondBorn = 0;

	// Use this for initialization
	void Start () {
        //should create colony
        antHillLink = (Instantiate(Resources.Load("Prefab/antHillPrefab"),Vector2.zero, Quaternion.identity) as GameObject).GetComponent<antHillScript>();
        antHillLink.addColonyScript (this.GetComponent<colonyScript>());
        
		totalNumberOfAnts = numberOfDormantAnts = GV.START_ANTS;
		foodStored = GV.START_FOOD;
		waterStored = GV.START_WATER;
	}

	void LateUpdate(){
		
		if (lastSecondBorn < Mathf.Floor (Time.time)) {
			lastSecondBorn = Mathf.Floor (Time.time);
			antsBornThisSecond = 0f;
			//Debug.Log ("Colony Health: " + averageAntErgy);
			if (averageAntErgy > 0.9) {
				while(antsBornThisSecond < GV.MAX_BORN_ANTS_PER_SEC){
					//Debug.Log ("Ant Born.");
					totalNumberOfAnts++;
					numberOfDormantAnts++;
					antsBornThisSecond++;
				}
			}
		}
	}

	// Update is called once per frame
	void Update () {
		//Debug.Log ("TotAnts " + totalNumberOfAnts);
		if (lastSecond < Mathf.Floor (Time.time)) {
			lastSecond = Mathf.Floor (Time.time);
			antsExitedThisSecond = 0;
		}

		//Debug.Log ("PercentDormantAnts: " + percentDormantAnts());
		if(percentDormantAnts() >= GV.DESIRED_PERCENT_DORMANT_ANTS){
			if (antsExitedThisSecond < GV.ANT_EXIT_PER_SECOND) {
				antsExitedThisSecond++;
				antExitsColony ();
			}
		}
		if (!antResourceDrainTracker && Mathf.Floor(Time.time + 1) % GV.COL_RESOURCE_DRAIN_TICK == 0) {
			antResourceDrainTracker = true;
			drainResources ();
		}else if(antResourceDrainTracker && Mathf.Floor(Time.time + 1) % GV.COL_RESOURCE_DRAIN_TICK != 0){
			antResourceDrainTracker = false;
		}
	}


	public void drainResources(){
		float tempDrain = numberOfDormantAnts * GV.COL_RESOURCE_DRAIN_DORMANT / 2;
		int starvingSeverity = 0;
		if (foodStored < tempDrain) {
			//running out of food
			Debug.Log ("Not enough food.");
			starvingSeverity++;
		} else
			foodStored -= tempDrain;
		if (waterStored < tempDrain) {
			Debug.Log("Not enough water.");
			starvingSeverity++;
		}else
			waterStored -= tempDrain;

		if (foodStored < 0 || waterStored < 0) {
			Debug.Log ("Colony is starving.");
			averageAntErgy -= averageAntErgy * GV.ENRGY_LOSS_FROM_STARV * starvingSeverity;
		}

		if (starvingSeverity == 0) {
			if (averageAntErgy > GV.ANT_ENERGY_MAX) {
				averageAntErgy = GV.ANT_ENERGY_MAX;
			} else {
				averageAntErgy = averageAntErgy + (GV.RESOURCE_TO_ENRGY * tempDrain / numberOfDormantAnts);
			}
		}
	}

	private void antDeathFromPoison(float quant){
		numberOfDormantAnts -= Mathf.CeilToInt(quant * GV.ANT_DEATH_FROM_POISON);
	}

	public void addResource(resourceObject resObj){
		if (!resObj.isZero ()) { //if we are holding things
			if (!resObj.isPoison) { //if that thing is poisoned
				if (resObj.resType == GV.ResourceTypes.Food) {  //ADD CORRESPONDING RESOURCE
					addFood (resObj.quantity);
				} else if (resObj.resType == GV.ResourceTypes.Water) {
					addWater (resObj.quantity);
				}
				//Debug.Log ("Food/Water added: Col FOOD/WATER = " + foodStored + " " + waterStored);
			} else {
				antDeathFromPoison(resObj.quantity);
			}
		}
	}

	private float percentDormantAnts(){
		//Debug.Log ("Here2: " + numberOfDormantAnts + " " + totalNumberOfAnts);
		return (float)numberOfDormantAnts / (totalNumberOfAnts + 1);
	}

	private void antExitsColony(){
		numberOfDormantAnts--;
		float tempAvg = averageAntErgy;
		if (tempAvg > 100f)
			tempAvg = 100f;
        if(totalAntsExited < GV.COLONY_NUM_SCOUT_SPAWN)
			antHillLink.antOut(tempAvg,40);
        else
			antHillLink.antOut (tempAvg,Random.Range(0,10));
        totalAntsExited++;
	}

	public void antEntersColony(GameObject ant){
		Ant antScript = ant.GetComponent<Ant> ();
		if(antScript != null){
			numberOfDormantAnts++;
			if(antScript.holding != null)
				addResource (antScript.giveResource());
			//Debug.Log ("(float) numberOfDormantAnts " + (float) numberOfDormantAnts);
			if (averageAntErgy > 100f)
				averageAntErgy = 100f;
			averageAntErgy = (averageAntErgy * (numberOfDormantAnts-1) + antScript.retEnergy ()) / (float) numberOfDormantAnts;
			//Debug.Log ("Colony Avrg En " + averageAntErgy + " averageAntErgy * totalNumberOfAnts " + averageAntErgy * totalNumberOfAnts + " antScript.retEnergy () " + antScript.retEnergy ());
			Destroy(ant);
		}
	}

	private void addFood(float input){
		foodStored += input;
	}

	private void addWater(float input){
		waterStored += input;
	}
}
