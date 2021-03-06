﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pursue : Seek {
    public float maxPrediction;
    protected GameObject targetAux;
    private GenericAgent targetAgent;

    public override void Awake() {
        base.Awake();
        targetAgent = target.GetComponent<GenericAgent>();
        targetAux = target;
        target = new GameObject();
    }

    void OnDestroy() {
        Destroy(targetAux);
    }

    public override Steering GetSteering() {
        Vector3 direction = targetAux.transform.position - transform.position;
        float distance = direction.magnitude;
        float speed = agent.velocity.magnitude;
        float prediction;

        if(speed <= distance / maxPrediction) {
            prediction = maxPrediction;
        } else {
            prediction = distance / speed;
        }

        target.transform.position = targetAux.transform.position + targetAgent.velocity * prediction;
        return base.GetSteering();
    }
}
