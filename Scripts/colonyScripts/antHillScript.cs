using UnityEngine;
using System.Collections;

public class antHillScript : MonoBehaviour{

	//link to colony object
	colonyScript colonyLink;

	public void Start(){

	}

	public void addColonyScript(colonyScript creatingScript){
		colonyLink = creatingScript;
	}

	public void antIn(GameObject ant){
		//report this to colonyScript
		colonyLink.antEntersColony(ant);
	}

	public void antOut(){
		//create ant at anthill
		GameObject ant = Instantiate (Resources.Load("Prefab/antPrefab") as GameObject,gameObject.transform.position,Quaternion.identity) as GameObject;
		//Instantiate (Resources.Load("Prefab/antPrefab") as GameObject,gameObject.transform.position,Quaternion.identity);
	}

	/*
	public void OnTriggerEnter2D(Collider2D coli){
		if (coli.gameObject.GetComponent<Ant> () != null) {
			//its an ant that entered the coli range
			if (true) { //if the ant actually wants to enter, assumed true
				antIn (coli.gameObject);
			}
		}
	} */
}
