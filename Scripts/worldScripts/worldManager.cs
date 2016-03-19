using UnityEngine;
using System.Collections;

public class worldManager : MonoBehaviour {
	private resourceAdminitrator resAdmin;
	private colonyScript colony;

	// Use this for initialization
	void Start () {
		//make resource administator
		resAdmin = gameObject.AddComponent<resourceAdminitrator>();
		colony = gameObject.AddComponent<colonyScript> ();

		//rocks
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
