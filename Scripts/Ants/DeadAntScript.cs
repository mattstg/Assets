using UnityEngine;
using System.Collections;

public class DeadAntScript : MonoBehaviour {
	float deathTime;
	// Use this for initialization
	void Start () {
		deathTime = Time.time;
	}
	
	// Update is called once per frame
	void LateUpdate () {
		if (deathTime + 15f <= Time.time)
			destroySelf ();	
	}

	private void destroySelf(){
		Destroy (gameObject);
	}

	void OnCollisionEnter2D(Collision2D coli){
		Ant temp = coli.collider.gameObject.GetComponent<Ant> ();
		if (temp != null) {
			if (!temp.hasCreatedFear) {
				temp.spawnFearPher (temp.transform.position, 100f);
				temp.hasCreatedFear = true;
			}
		}
	}

}
