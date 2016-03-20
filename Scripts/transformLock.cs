using UnityEngine;
using System.Collections;

public class transformLock : MonoBehaviour {

    public Transform toFollow;
    public Vector3 offset = new Vector3(0, 0, 0);

    public void Initialize(Transform parentLock)
    {
        toFollow = parentLock;
    }
	
	// Update is called once per frame
	void Update () {
        if (toFollow)
            transform.position = toFollow.position + offset; 
	}
}
