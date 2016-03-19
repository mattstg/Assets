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
		//Debug.Log("here");
		colonyLink.antEntersColony(ant);
	} 

	public void antOut(float avrgAntHealth){
		//create ant at anthill
		GameObject ant = Instantiate (Resources.Load("Prefab/antPrefab") as GameObject,gameObject.transform.position,Quaternion.identity) as GameObject;
		ant.GetComponent<Ant> ().setHealth(avrgAntHealth);
		//ant.GetComponent<Ant> ()
		Debug.Log(">>>>>>>>>>>>>" + ant.GetComponent<Ant> ().wantsToEnterHive);
		//Instantiate (Resources.Load("Prefab/antPrefab") as GameObject,gameObject.transform.position,Quaternion.identity);
	}
		
	public void OnTriggerEnter2D(Collider2D coli){
		Ant tempAnt = coli.gameObject.GetComponent<Ant> ();
		if (tempAnt != null) {
			//its an ant that entered the coli range
			Debug.Log(tempAnt.wantsToEnterHive);
			if (tempAnt.wantsToEnterHive) { //if the ant actually wants to enter, assumed true
				Debug.Log("AGGGGGGGGGGGGGGGGGGGGGGG");
				antIn (tempAnt.gameObject);
			}
		}
	}
}
