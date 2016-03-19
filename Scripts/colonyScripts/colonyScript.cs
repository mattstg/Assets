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
		//ants dont consume food while inside ATM
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
				//food is poisoned, do something

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
