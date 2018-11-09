using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackForthBehaviour : IBehaviour {
    [SerializeField] public Vector3 distance;

    private Vector3 startPoint, endPoint;

    public override void Start() {
        base.Start();
        startPoint = character.transform.position;
        endPoint = startPoint + distance;
    }

    public override void MovementIntent() {
        /* qui diremo come dovrà muoversi */
        Vector3 velocity = endPoint - character.transform.position;
        if (velocity.magnitude >= 0.2f) {
            velocity.Normalize();
            velocity *= character.moveSpeed;
            if (velocity.magnitude > 0) {
                character.transform.rotation = Quaternion.Lerp(character.transform.rotation,
                    Quaternion.Euler(new Vector3(0.0f, Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg, 0.0f)),
                    character.rotationSpeed * Time.deltaTime);
            }
            character.transform.position += velocity * Time.deltaTime;
        } else {
            Vector3 tmp = startPoint;
            startPoint = endPoint;
            endPoint = tmp;
        }
    }
}
