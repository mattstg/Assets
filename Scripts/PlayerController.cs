using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public float playerEnergy = GV.PLAYER_MAX_HEALTH;
    bool onTheMove = false;
    Vector2 goalLocation;
    Rigidbody2D rbod;
    PheromoneNode lastNode;
    float zoomValue = 15;


    void Start()
    {
        rbod = GetComponent<Rigidbody2D>();
        playerEnergy = GV.PLAYER_MAX_HEALTH;
    }

    public void Update()
    {
        float dtime = Time.deltaTime;
        if (Input.GetMouseButtonDown(1))
            RightClick();
        if (onTheMove)
        {
            MoveTowardsGoal(dtime);
            IfReachedGoal();
        }
        Zoom();
        playerEnergy += GV.PLAYER_E_REGEN * dtime;
        playerEnergy = (playerEnergy > GV.PLAYER_MAX_HEALTH)?GV.PLAYER_MAX_HEALTH:playerEnergy;
        if (playerEnergy < 0)
            Die();
        if (Input.GetKeyDown(KeyCode.P))
            SuperMath();
    }

    void SuperMath()
    {
        System.Collections.Generic.List<Intersection> intersections = GameObject.FindObjectOfType<PheromoneManager>().GetIntersections(transform.position, GetMousePosition());
        foreach (Intersection intrs in intersections)
        {
            GameObject go = Instantiate(Resources.Load("Prefab/Marker")) as GameObject;
            go.transform.position = intrs._intersectionPoint;
        }
    }

    void IfReachedGoal()
    {
        onTheMove = !((int)goalLocation.x == (int)transform.position.x && (int)goalLocation.y == (int)transform.position.y);
        if (!onTheMove)
            rbod.velocity = new Vector2(0, 0);
    }

    void MoveTowardsGoal(float dtime)
    {
        float angle = 0;
        Vector2 velo = GV.SubtractVectors(goalLocation, transform.position).normalized * GV.PLAYER_MOVE_SPEED;
        rbod.velocity = velo;
        playerEnergy -= GV.PLAYER_MOVE_E_COST* dtime;
        if (velo.y < 0f)
            angle = 180;
        if (velo.y != 0)
            angle = angle + -Mathf.Atan(velo.x / velo.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, angle);
    }

    void Zoom()
    {
        float wheelValue = Input.GetAxis("Mouse ScrollWheel");
        if (wheelValue != 0)
        {
            zoomValue += wheelValue * GV.PLAYER_ZOOM_SPEED;
            if(zoomValue >  GV.PLAYER_ZOOM_MAX)
                zoomValue = GV.PLAYER_ZOOM_MAX;
            else if (zoomValue < GV.PLAYER_ZOOM_MIN)
                zoomValue = GV.PLAYER_ZOOM_MIN;
            Camera.main.orthographicSize = zoomValue;
        }
    }

    void RightClick()
    {
        if (playerEnergy > GV.PLAYER_CLICK_E_COST)
        {
            bool bnsDmg = lastNode != null;
            onTheMove = true;
            goalLocation = GetMousePosition();
            playerEnergy -= GV.PLAYER_CLICK_E_COST;
            lastNode = PheromoneManager.DropPheromone(lastNode,GV.PhermoneTypes.Friendly,transform.position);
            if (bnsDmg && lastNode.trails[0] != null)
                lastNode.trails[0].strength = (int)GV.PLAYER_PHER_START;
            
        }
    }

    void OnCollisionStay2D(Collision2D coli)
    {
        if (coli.gameObject.CompareTag("Node"))
        {
            lastNode = coli.gameObject.GetComponent<PheromoneNode>();
        }
        else if (coli.gameObject.GetComponent<Ant>())
        {
            coli.gameObject.GetComponent<Ant>().takeDamage(GV.PLAYER_DMG*Time.deltaTime);
            playerEnergy -= GV.PLAYER_DMG_TAKEN*Time.deltaTime;
        }
    }

    void Die()
    {
        Destroy(Camera.main.GetComponent<transformLock>());
        Camera.main.orthographicSize = 35;
        Destroy(this.gameObject);
    }

    Vector2 GetMousePosition()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    
}
