using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class resourceAdminitrator : MonoBehaviour {
	private List<waterPuddleScript> waterRes = new List<waterPuddleScript>();
	private List<foodBundleScript> foodRes = new List<foodBundleScript>();
	float halfMapSize = GV.Map_Diameter/2;
	private GameObject waterPrefab = Resources.Load("Prefab/waterPuddlePrefab") as GameObject;
	private GameObject foodPrefab = Resources.Load("Prefab/foodBundlePrefab") as GameObject;

	// Use this for initialization
	void Start () {
		generateResources ();
	}
	
	// Update is called once per frame
	void Update () {
		//update resources
		if(Mathf.Floor(Time.time) % GV.TIME_BETWEEN_RESOURCE_UPDATES == 0)
			updateResources();
	}

	void generateResources(){
		//to be run at start to generate resources
		createFoodSource(GV.Max_FoodBundles);
		createWaterSource (GV.Max_WaterPuddes);
	}
	
	private void createWaterSource(int amountToAdd){
		for(int count = 0; count < amountToAdd; count++){
			Vector3 rand = new Vector3 (Random.Range (-halfMapSize, halfMapSize), Random.Range (-halfMapSize, halfMapSize), 0);
			while (locationIsEmpty(rand) == false) {
				rand = new Vector3 (Random.Range (-halfMapSize, halfMapSize), Random.Range (-halfMapSize, halfMapSize), 0);
			}
			GameObject newWater = Instantiate (waterPrefab, rand, Quaternion.identity) as GameObject;
			waterRes.Add (newWater.GetComponent<waterPuddleScript>());
		}
	}

	private void createFoodSource(int amountToAdd){
		for(int count = 0; count < amountToAdd; count++){
			Vector3 rand = new Vector3 (Random.Range (-halfMapSize, halfMapSize), Random.Range (-halfMapSize, halfMapSize), 0);
			while (locationIsEmpty(rand) == false) {
				rand = new Vector3 (Random.Range (-halfMapSize, halfMapSize), Random.Range (-halfMapSize, halfMapSize), 0);
			}
			GameObject newFood = Instantiate (foodPrefab, rand, Quaternion.identity) as GameObject;
			foodRes.Add (newFood.GetComponent<foodBundleScript>());
		}
	}

	public int calculateGlobalFood(){
		int sum = 0;
		foreach(foodBundleScript bundle in foodRes){
			sum += bundle.retQuant ();
		}
		return sum;
	}

	public int calculateGlobalWater(){
		int sum = 0;
		foreach(waterPuddleScript puddle in waterRes){
			sum += puddle.retQuant ();
		}
		return sum;
	}

	private void updateResources(){
		foreach(foodBundleScript bundle in foodRes){
			bundle.manualUpdate ();
		}
		foreach(waterPuddleScript puddle in waterRes){
			puddle.manualUpdate ();
		}
	}

	/*
	private bool locationIsEmpty(Vector2 vec){
		RaycastHit2D testLoc = Physics2D.Raycast (vec, LayerMask.NameToLayer("Resource"));
		if (testLoc.collider == null) {
			return true;
		} else
			return false;
	} */

	private bool locationIsEmpty(Vector3 vec){
		return false;
	} 
}
