using UnityEngine;
using System.Collections;

public class worldManager : MonoBehaviour {
	private resourceAdminitrator resAdmin;
	private colonyScript colony;
	float halfMapSize = GV.MAP_DIAMETER/2;

	// Use this for initialization
	void Start () {
		//make resource administator
		createRocks((int) Random.Range(GV.NUM_OF_ROCKS.x,GV.NUM_OF_ROCKS.y));
		colony = gameObject.AddComponent<colonyScript> ();
		resAdmin = gameObject.AddComponent<resourceAdminitrator>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private void createRocks(int rocksToMake){
		GameObject rockPrefab = Resources.Load ("Prefab/rockPrefab") as GameObject;
		for(int count = 0; count < rocksToMake; count++){
			Vector3 rand = new Vector3 (Random.Range (-halfMapSize, halfMapSize), Random.Range (-halfMapSize, halfMapSize), 0);
			while (Mathf.Abs (rand.x) < GV.ROCK_SPAWN_RANGE_FROM_ORIG || Mathf.Abs (rand.y) < GV.ROCK_SPAWN_RANGE_FROM_ORIG) {
				rand =  new Vector3 (Random.Range (-halfMapSize, halfMapSize), Random.Range (-halfMapSize, halfMapSize), 0);
			}
			GameObject newRock = Instantiate (rockPrefab, rand, Quaternion.identity) as GameObject;
			float tempScale = Random.Range (GV.RANGE_OF_ROCK_SCALE.x, GV.RANGE_OF_ROCK_SCALE.y);
			newRock.transform.localScale = new Vector3 (tempScale, tempScale, 0);
		}

	}
}
