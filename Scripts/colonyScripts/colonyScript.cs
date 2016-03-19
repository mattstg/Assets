using UnityEngine;
using System.Collections;

public class colonyScript : MonoBehaviour {

	//antHill
	antHillScript antHillLink;

	//Vars
	public int totalNumberOfAnts;
	private int numberOfDormantAnts;
	private int foodStored;
	private int waterStored;

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
		if(Mathf.Floor(Time.time) % GV.TIME_BETWEEN_ANT_EXITS == 0 && percentDormantAnts() > GV.DESIRED_PERCENT_DORMANT_ANTS){
			antExitsColony ();
		}
	}

	public void addFood(){
		foodStored++;
	}

	public void addWater(){
		waterStored++;
	}

	private float percentDormantAnts(){
		return numberOfDormantAnts / totalNumberOfAnts;
	}

	private void antExitsColony(){
		numberOfDormantAnts--;
		antHillLink.antOut ();
	}

	public void antEntersColony(GameObject ant){
		numberOfDormantAnts++;
		//if holding stuff add that stuff

		//destoy ant obj
		Destroy(ant);
	}
}
