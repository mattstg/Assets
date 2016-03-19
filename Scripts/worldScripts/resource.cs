using UnityEngine;
using System.Collections;

public class resource : MonoBehaviour {
	private float quantity;
	private bool isPoison;


	// Use this for initialization
	void Start () {
		quantity = Random.Range (GV.INITIAL_RESOURCE_RANGE.x, GV.INITIAL_RESOURCE_RANGE.y);
	}

	// Update is called once per frame
	/*
	void Update () {
		
	} */

	public void manualUpdate(){
		if (quantity <= 0)
			Destroy (gameObject);
		quantity += GV.RESOURCE_GROWTH_PER_SECOND * Time.deltaTime;
	}

	public void give(Ant ant){
		//give ant the food type
		quantity -= GV.ANT_CARRY_CAPACITY;
	}

	public float retQuant(){
		return quantity;
	}
}
