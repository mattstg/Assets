using UnityEngine;
using System.Collections;

public class fearScript : MonoBehaviour {
	public float strength;

	// Use this for initialization
	void Start () {
		if(strength == null)
			strength = 100f;

		gameObject.transform.localScale = new Vector3 (GV.FEAR_SIZE, GV.FEAR_SIZE, GV.FEAR_SIZE);
	}

	void LateUpdate(){
		if (strength < 0)
			Destroy (gameObject);
		else {
			float temp = strength/100*GV.FEAR_SIZE;
			gameObject.transform.localScale = new Vector3 (strength/100*GV.FEAR_SIZE, strength/100*GV.FEAR_SIZE, 0);
		}
	}

	// Update is called once per frame
	void Update () {
		strength -= GV.FEAR_DECAY_PER_SEC * Time.deltaTime;
	}

	void OnTriggerEnter2D(Collider2D coli){
		Ant temp = coli.GetComponent<Ant> ();
		if (temp != null) {
			//
			if (strength > 25f) {
				temp.spawnFearPher (temp.transform.position, strength - 25f);
				temp.hasCreatedFear = true;
			}
			//temp.DropPheromone ();
			temp.MoveTowardsGoal (new Vector2(gameObject.transform.position.x,gameObject.transform.position.y));
		}
	}
}
