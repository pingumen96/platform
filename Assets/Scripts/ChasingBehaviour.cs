using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingBehaviour : IBehaviour {
    [SerializeField] public GameObject target;
    [SerializeField] public float triggerDistance;

    private Vector3 origin;

    public override void Start() {
        base.Start();
        origin = character.transform.position;
    }

    public override void MovementIntent() {
        /* qui diremo come dovrà muoversi */
        Vector3 destination = Vector3.Distance(origin, target.transform.position) <= triggerDistance ? target.transform.position : origin;
        Vector3 velocity = destination - character.transform.position;
        velocity.y = 0.0f;
        if(velocity.magnitude >= 0.5f) {
            velocity.Normalize();
            velocity *= character.moveSpeed;
            if(velocity.magnitude > 0) {
                character.transform.rotation = Quaternion.Lerp(character.transform.rotation,
                    Quaternion.Euler(new Vector3(0.0f, Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg, 0.0f)),
                    character.rotationSpeed * Time.deltaTime);
            }
            character.transform.position += velocity * Time.deltaTime;
        }
    }
}
