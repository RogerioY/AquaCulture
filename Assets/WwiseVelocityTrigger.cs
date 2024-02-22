using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseVelocityTrigger : MonoBehaviour {
    float objectVelocity;
	// Use this for initialization
	void Start () {
        objectVelocity = GetComponent<Rigidbody2D>().velocity.magnitude;
	}
	
	// Update is called once per frame
	void Update () {
        if (objectVelocity != 0) { Debug.Log("moving"); }
	}
}
