using UnityEngine;
using System.Collections;

public class worldManager : MonoBehaviour {
	private resourceAdminitrator resAdmin;
	private colonyScript colony;

	// Use this for initialization
	void Start () {
		//make resource administator
		colony = gameObject.AddComponent<colonyScript> ();
		resAdmin = gameObject.AddComponent<resourceAdminitrator>();
		//rocks
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
