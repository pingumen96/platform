using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackForth : Seek {
    [SerializeField] public Vector3 distance;

    private Vector3 startPoint, endPoint;

    public override void Awake() {
        base.Awake();
        startPoint = transform.position;
        endPoint = startPoint + distance;
        target = new GameObject();
        target.transform.position = endPoint;
    }

    public override Steering GetSteering() {
        if (Vector3.Distance(transform.position, target.transform.position) < 0.1f) {
            target.transform.position = target.transform.position == startPoint ? endPoint : startPoint;
        }
        return base.GetSteering();
    }

    void OnDestroy() {
        Destroy(target);
    }
}