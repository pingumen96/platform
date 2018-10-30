using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision) {
        Debug.Log("collisione");
        if(collision.gameObject.CompareTag("Player")) {
            collision.gameObject.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit(Collision collision) {
        if(collision.gameObject.CompareTag("Player")) {
            collision.gameObject.transform.SetParent(null);
        }
    }
}
