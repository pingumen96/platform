using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAgent : GenericAgent {
    public float maxAcceleration;
    public float maxRotation; // rappresenta in qualche modo un attrito angolare
    public bool /*IBelieveI*/ canFly = false;

    public override void Update () {
        orientation += rotation * Time.deltaTime;

        // i valori di orientation devono rientrare nel range 0 - 360
        if(orientation < 0.0f) {
            orientation += 360.0f;
        } else if(orientation > 360.0f) {
            orientation -= 360.0f;
        }
        
        transform.Translate(velocity * Time.deltaTime, Space.World);
        transform.rotation = new Quaternion();
        transform.Rotate(Vector3.up, orientation);
	}

    // calcoliamo lo steering per il prossimo frame
    public override void LateUpdate() {
        velocity += steering.linear * Time.deltaTime;
        rotation += steering.angular * Time.deltaTime;

        if (!canFly && steering.linear.y > transform.position.y) {
            velocity.y = transform.position.y;
        }

        if(velocity.magnitude > maxSpeed) {
            velocity.Normalize();
            velocity *= maxSpeed;
        }

        if(steering.angular == 0.0f) {
            rotation = 0.0f;
        }

        if(steering.linear.sqrMagnitude == 0.0f) {
            velocity = Vector3.zero;
        }

        if (rotation > maxRotation) {
            rotation = maxRotation;
        }

        steering = new Steering();
    }
}
