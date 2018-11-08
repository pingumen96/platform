using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingBehaviour : IBehaviour {
    [SerializeField] public GameObject target;
    [SerializeField] public float triggerDistance;

    private float distance;
    private Vector3 origin;

    public override void MovementIntent() {
        /* qui diremo come dovrà muoversi */
        Debug.Log("chasing player");
    }
}
