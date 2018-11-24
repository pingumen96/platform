using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericAgent : MonoBehaviour {
    public float maxSpeed;
    public float orientation;
    public float rotation;
    public Vector3 velocity;
    protected Steering steering;

    void Start() {
        velocity = Vector3.zero;
        steering = new Steering();
    }
    
    public void SetSteering(Steering steering) {
        this.steering = steering;
    }

    public virtual void Update() {

    }

    public virtual void LateUpdate() {

    }
}
