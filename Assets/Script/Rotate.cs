using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour {
    float y;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        y += 0.5f;
        this.transform.rotation = Quaternion.Euler(new Vector3(this.transform.rotation.x, y, this.transform.rotation.z));
	}
}
