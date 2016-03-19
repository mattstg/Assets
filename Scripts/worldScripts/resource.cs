using UnityEngine;
using System.Collections;

public class resource : MonoBehaviour {
	public float quantity;
	public bool isPoison;
	public GV.ResourceTypes resourceType = GV.ResourceTypes.Empty;

	// Use this for initialization
	void Start () {
		quantity = 0f;
		addQuantity(Random.Range (GV.INITIAL_RESOURCE_RANGE.x, GV.INITIAL_RESOURCE_RANGE.y));
		if (Random.Range (0, 1) < GV.PERCENT_CHANCE_OF_POISON)
			isPoison = true;
		else
			isPoison = false;
	}

	public void manualUpdate(){
		if (quantity <= 0)
			Destroy (gameObject);
		addQuantity(GV.RESOURCE_GROWTH_PER_SECOND * Time.deltaTime);
	}

	public void give(Ant ant){
		//give ant the food type
		ant.takeResource(new resourceObject(resourceType, GV.ANT_CARRY_CAPACITY, isPoison));
		addQuantity(-GV.ANT_CARRY_CAPACITY);
		if(quantity <= 0)
			Destroy(gameObject);
	}

	public float retQuant(){
		if (isPoison = true) 
			return quantity * -1f;
		return quantity;
	}

	public void poisonResource(bool toSet){
		isPoison = toSet;
	}

	public void addQuantity(float toAdd){
		quantity += toAdd;
		rescale ();
	}

	public void rescale(){
		float newScale = Mathf.Sqrt(quantity / GV.QUANTITY_TO_VOLUMETIC_SCALE);
		gameObject.transform.localScale = new Vector2 (newScale,newScale);
	}

	public void OnTriggerEnter2D(Collider2D coli){
		Ant collidingAnt = coli.gameObject.GetComponent<Ant> ();
		if (collidingAnt != null) {
			give (collidingAnt);
		}
	}
}
