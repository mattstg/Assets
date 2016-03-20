using UnityEngine;
using System.Collections;
using System.Linq;

public class DEBUG_CreateTrails : MonoBehaviour {
    public PheromoneManager pherManager;
    public PheromoneNode initialNode;
    public PheromoneNode lastClicked;
	// Use this for initialization
	void Start () {
        /*
        Vector2 randomSpot = new Vector2(Random.Range(-5, 5), Random.Range(-5, 5));
        PheromoneNode pn = pherManager.CreateNewNode(initialNode, GV.PhermoneTypes.Friendly, randomSpot);
         * */
	}

    public void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            GameObject newAnt = Instantiate(Resources.Load("Prefab/AntPrefab"),new Vector3(0,0,0),Quaternion.identity) as GameObject;
            newAnt.GetComponent<Ant>().Initialize();
        }
    }

    public void OnMouseDown()
    {
        Vector2 clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(clickPoint, Vector2.zero);
        PheromoneNode clickedNode = null;
        if (hits.Length != 0)
        {
            foreach (RaycastHit2D rhit in hits)  //def better way of doing this, but debug function
            {
                if (rhit.transform.CompareTag("Node"))
                {
                    clickedNode = rhit.collider.gameObject.GetComponent<PheromoneNode>();
                }
            }
        }

        if (clickedNode)  //if not null
        {
            SetNewLastClicked(clickedNode);
        }
        else  //otherwise make a new one
        {
            SetNewLastClicked(PheromoneManager.DropPheromone(lastClicked, GV.PhermoneTypes.Friendly, clickPoint));
        }
        
    }

    private void SetNewLastClicked(PheromoneNode pn)
    {
        if(lastClicked)
            lastClicked.GetComponent<SpriteRenderer>().color = Color.green;
        pn.GetComponent<SpriteRenderer>().color = Color.red;
        lastClicked = pn;
    }

    private void StraightUpAnt(Vector2 startPos)
    {
        //GameObject newAnt = Instantiate(Resources.Load("Prefab/AntPrefab"), startPos, Quaternion.identity) as GameObject;
        //newAnt.GetComponent<Ant>().Initialize();
        //newAnt.GetComponent<Ant>().goal
    }
}
