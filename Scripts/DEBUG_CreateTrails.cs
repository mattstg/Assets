using UnityEngine;
using System.Collections;
using System.Linq;

public class DEBUG_CreateTrails : MonoBehaviour {
    public PheromoneManager pherManager;
    public PheromoneNode initialNode;
    public PheromoneNode lastClicked;
	// Use this for initialization
	void Start () {
        PheromoneNode pn = pherManager.RetrieveNewNode(initialNode, GV.PhermoneTypes.Friendly);
        Vector2 randomSpot = new Vector2(Random.Range(-5,5),Random.Range(-5,5));
        pn.transform.position = randomSpot;
        
	}

    public void OnMouseDown()
    {
        Debug.Log("clicked");
        Vector2 clickPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(clickPoint, Vector2.zero);
        Debug.Log("Mouse clicked on: " + clickPoint);
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
            PheromoneNode pn = pherManager.RetrieveNewNode(lastClicked, GV.PhermoneTypes.Friendly);
            pn.transform.position = clickPoint;
            //Debug.Log("Mouse position is" + Input.mousePosition);
            SetNewLastClicked(pn);
        }
        
    }

    private void SetNewLastClicked(PheromoneNode pn)
    {
        lastClicked.GetComponent<SpriteRenderer>().color = Color.green;
        pn.GetComponent<SpriteRenderer>().color = Color.red;
        lastClicked = pn;
    }

}
