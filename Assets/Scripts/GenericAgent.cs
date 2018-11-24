using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericAgent : MonoBehaviour {
    public float maxSpeed;
    public float maxAngularAcceleration;
    public float orientation;
    public float rotation;
    public Vector3 velocity;
    protected Steering steering;

    void Start() {
        velocity = Vector3.zero;
        steering = new Steering();
    }
    
    public void SetSteering(Steering steering, float weight) {
        this.steering.linear += weight * steering.linear;
        this.steering.angular += weight * steering.angular;
    }

    public virtual void Update() {

    }

    public virtual void LateUpdate() {

    }
}
