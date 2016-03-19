using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class resourceAdminitrator : MonoBehaviour {
	private List<waterPuddleScript> waterRes = new List<waterPuddleScript>();
	private List<foodBundleScript> foodRes = new List<foodBundleScript>();
	float halfMapSize = GV.MAP_DIAMETER/2;
	private GameObject waterPrefab = Resources.Load("Prefab/waterPuddlePrefab") as GameObject;
	private GameObject foodPrefab = Resources.Load("Prefab/foodBundlePrefab") as GameObject;

	// Use this for initialization
	void Start () {
		generateResources ();
	}
	
	// Update is called once per frame
	void Update () {
		updateResources();

		//shouldnt be done every single update :S

		 if (calculateGlobalFood () < GV.MIN_WORLD_FOOD) {
			createFoodSource (1);
		}

		if (calculateGlobalWater () < GV.MIN_WORLD_WATER)
			createWaterSource (1);
	}

	void generateResources(){
		//to be run at start to generate resources
		createFoodSource((int) Random.Range(GV.NUM_FOOD.x,GV.NUM_FOOD.y));
		createWaterSource ((int) Random.Range(GV.NUM_WATER.x,GV.NUM_WATER.y));
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

	public float calculateGlobalFood(){
		float sum = 0;
		foreach(foodBundleScript bundle in foodRes){
//			Debug.Log (bundle.toString() + " Sum: " + sum);
			sum += bundle.retQuant ();
		}
		return sum;
	}

	public float calculateGlobalWater(){
		float sum = 0;
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
		return true;
	} 
}
