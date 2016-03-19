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
	private bool antOutPutLimiter = false;
	private bool antResourceDrainTracker = false;

	// Use this for initialization
	void Start () {
		//should create colony
		antHillLink = (Instantiate(Resources.Load("Prefab/antHillPrefab"),Vector2.zero, Quaternion.identity) as GameObject).GetComponent<antHillScript>();
		antHillLink.addColonyScript (this.GetComponent<colonyScript>());
		totalNumberOfAnts = numberOfDormantAnts = GV.START_ANTS;
		foodStored = GV.START_FOOD;
		waterStored = GV.START_WATER;
	}

	// Update is called once per frame
	void Update () {
		//Debug.Log ("Here" + percentDormantAnts() +  GV.DESIRED_PERCENT_DORMANT_ANTS);
		if(percentDormantAnts() >= GV.DESIRED_PERCENT_DORMANT_ANTS){
			antExitsColony ();
		}
	

		if (!antResourceDrainTracker && Mathf.Floor(Time.time + 1) % GV.RESOURCE_DRAIN_TICK == 0) {
			Debug.Log ("Inside, once ever 6 seconds.");
			antResourceDrainTracker = true;
			drainResources ();
		}else if(antResourceDrainTracker && Mathf.Floor(Time.time + 1) % GV.RESOURCE_DRAIN_TICK != 0){
			antResourceDrainTracker = false;
		}
	}


	public void drainResources(){
		float tempDrain = numberOfDormantAnts * GV.RESOURCE_DRAIN_DORMANT / 2;
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
		}
	}

	private void someAntStarve(int strvSvrity){
		numberOfDormantAnts -= Mathf.CeilToInt(strvSvrity * GV.ANT_DEATH_FROM_STARVATION);
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
			} else {
				antDeathFromPoison(resObj.quantity);
			}
		}
	}

	private float percentDormantAnts(){
		//Debug.Log ("Here2: " + numberOfDormantAnts + " " + totalNumberOfAnts);
		return (float) numberOfDormantAnts / totalNumberOfAnts;
	}

	private void antExitsColony(){
		numberOfDormantAnts--;
		antHillLink.antOut ();
	}

	public void antEntersColony(GameObject ant){
		Ant antScript = ant.GetComponent<Ant> ();
		if(antScript != null){
			numberOfDormantAnts++;
			addResource (antScript.giveResource());
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
