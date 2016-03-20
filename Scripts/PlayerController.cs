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
}
