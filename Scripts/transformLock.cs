using UnityEngine;
using System.Collections;

public class transformLock : MonoBehaviour {

    Transform toFollow;

    public void Initialize(Transform parentLock)
    {
        toFollow = parentLock;
    }
	
	// Update is called once per frame
	void Update () {
        if (toFollow)
            transform.position = toFollow.position;
	}
}
