using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float playerEnergy = GV.PLAYER_MAX_HEALTH;
    Vector2 goalLocation;
    Rigidbody2D rbod;

    void Start()
    {
        rbod = GetComponent<Rigidbody2D>();
    }

    public void Update()
    {
		if (Input.GetKeyDown(o)) {
			spawnFearPher (gameObject.transform.localPosition + new Vector3(0f, 0.3f, 0f),100f);
		}

        float dtime = Time.deltaTime;
        if (Input.GetMouseButtonDown(1))
            RightClick();

    }

    void MoveTowardsGoal(float dtime)
    {
        
        rbod.velocity = GV.SubtractVectors(goalLocation, transform.position).normalized*GV.PLAYER_MOVE_SPEED*dtime;
    }

    void RightClick()
    {
        if (playerEnergy > GV.PLAYER_CLICK_E_COST)
            goalLocation = GetMousePosition();
    }

    Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

	public void spawnFearPher(Vector3 loc, float strIN){
		if (strIN > 11) {
			GameObject holder = Instantiate (Resources.Load ("Prefab/PharamoneFear"), loc, Quaternion.identity) as GameObject;
			holder.GetComponent<fearScript> ().strength = strIN - 10;
		}
	}
}
