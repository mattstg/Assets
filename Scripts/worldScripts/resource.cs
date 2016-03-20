using UnityEngine;
using System.Collections;

public class resource : MonoBehaviour {
	public float quantity;
	public bool isPoison;
	public GV.ResourceTypes resourceType;
	private bool toDestroy = false;


	void LateUpdate(){
		if (toDestroy)
			destroyThis ();
	}

	// Use this for initialization
	void Start () {
		quantity = 0f;
		addQuantity(Random.Range (GV.INITIAL_RESOURCE_RANGE.x, GV.INITIAL_RESOURCE_RANGE.y));
		float randomValue = Random.value;
		if (randomValue <= GV.PERCENT_CHANCE_OF_POISON) {
			//Debug.Log ("Is Poisoned. randomValue: " + randomValue);
			isPoison = true;
		}
		else{
			//Debug.Log ("Is NOT Poisoned. randomValue: " + randomValue);
			isPoison = false;
		}
//		Debug.Log ("New Resource: " + toString());
		if (Random.Range(-10,10) < 0) {
			gameObject.GetComponent<SpriteRenderer> ().flipX = true;
		}
		if(Random.Range(-10,10) < 0){
			gameObject.GetComponent<SpriteRenderer>().flipY = true;
		}
	}

	public void manualUpdate(){
		addQuantity(GV.RESOURCE_GROWTH_PER_SECOND * GV.TIME_BETWEEN_RESOURCE_UPDATES);
	}

	public void give(Ant ant, Transform resLoc){
		//give ant the food type
		ant.takeResource(new resourceObject(resourceType, GV.ANT_CARRY_CAPACITY, isPoison),resLoc);
		addQuantity(-GV.ANT_CARRY_CAPACITY);
	}

	public float retQuant(){
		if (isPoison) 
			return quantity * -1f;
		return quantity;
	}

	public void poisonResource(bool toSet){
		isPoison = toSet;
	}

	public void addQuantity(float toAdd){
		quantity += toAdd;
		if(quantity > 0)
			rescale ();
	}

	public void rescale(){
		float newScale;
		if (quantity > 0) {
			newScale = Mathf.Sqrt(Mathf.Abs(quantity) / GV.QUANTITY_TO_VOLUMETIC_SCALE);
		} else {
			newScale = 0;
		}
		if(gameObject != null)
			gameObject.transform.localScale = new Vector2 (newScale,newScale);
	}

	public void OnTriggerEnter2D(Collider2D coli){
		if (coli.gameObject.GetComponent<Ant> () != null) {
			Ant collidingAnt = coli.gameObject.GetComponent<Ant> ();
			//if (collidingAnt)
			if (collidingAnt.holding == null || collidingAnt.holding.isZero()) 
				give (collidingAnt, gameObject.transform);
		} else {
			//Debug.Log ("Something has collided with a resource, that is not an ant. This is it: " + coli.name);
		}
	}

	public string toString(){
		return "ResType: " + resourceType + " quantity: " + quantity + " isPoisoned " + isPoison; 
	}

	private void destroyThis(){
		Destroy (gameObject);
	}

	public void markToDie(){
		toDestroy = true;
	}
}
