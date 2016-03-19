using UnityEngine;
using System.Collections;

public class Ant : MonoBehaviour {

	//Var
	private resourceObject holding;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public resourceObject giveResource(){
		//pass the thing to ANT or colony
		return holding.give();
	}

	public resourceObject whatResourceHolding(){
		return holding;
	}

	public void takeResource(resourceObject resourceToHold){
		holding = resourceToHold;
	}
}
