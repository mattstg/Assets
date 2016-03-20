using UnityEngine;
using System.Collections;

public class Cordyceps : MonoBehaviour {
    Ant ant;
	// Use this for initialization
	void Start () {
        ant = GetComponent<Ant>();
	}
	
	// Update is called once per frame
	void Update () {
        ant.setEnergy(100);
        if (Input.GetMouseButtonDown(1))
        {
            Vector3 pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            ant.DEBUG_CordycepControl(pz);
        }
	}
}
