using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingEnemy : Character {
    [SerializeField] public GameObject target;
    [SerializeField] public float triggerDistance;

    private float distance;
    private Vector3 origin;

	// Use this for initialization
	new void Start () {
        base.Start();
        origin = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    protected override void OnControllerColliderHit(ControllerColliderHit hit) {
        base.OnControllerColliderHit(hit);
        // quando toglie una vita al personaggio, torna all'origine per un po' di tempo e ricomincia
    }
}
