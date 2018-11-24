using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Pursue {
    public float triggerRadius;

    private Vector3 origin;

    public override void Awake() {
        base.Awake();
        origin = transform.position;
    }

    public override Steering GetSteering() {
        if (Vector3.Distance(targetAux.transform.position, origin) > triggerRadius) {
            return new Steering();
        } else {
            return base.GetSteering();
        }
    }
}
