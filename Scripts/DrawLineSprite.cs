using UnityEngine;
using System.Collections;

public class DrawLineSprite : MonoBehaviour {

    Vector2 v1, v2;
    public Sprite sprite;
	// Update is called once per frame
    public void Initialize(Vector2 _v1, Vector2 _v2)
    {
        v1 = _v1;
        v2 = _v2;
        gameObject.GetComponent<SpriteRenderer>().sprite = sprite;
    }
    
    void Update () {
        DrawRenderer();
	}

    private void DrawRenderer()
    {
        if (v1 != null && v2 != null && v1 != v2)
        {
            Vector2 vecDiff = v2 - v1;
            float ang = Mathf.Atan(vecDiff.x / vecDiff.y) * Mathf.Rad2Deg * -1;            
            transform.position = ((v2 - v1) / 2) + v1;
            transform.localScale = new Vector3(1, Vector2.Distance(v1, v2), 1);
            transform.eulerAngles = new Vector3(0, 0, ang);
        }/*
        else
        {
            Destroy(this);
        }*/

    }
}
