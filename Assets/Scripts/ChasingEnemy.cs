using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasingEnemy : MonoBehaviour {
    [SerializeField] public GameObject target;
    [SerializeField] public float speed;
    [SerializeField] public float triggerDistance;
    [SerializeField] public bool /*IbelieveI*/canFly;

    private float distance;
    private Vector3 origin;
    private bool hitGround;
    private Collider collider;

	// Use this for initialization
	void Start () {
        origin = transform.position;
        collider = GetComponent<BoxCollider>();
	}
	
	// Update is called once per frame
	void Update () {
        RaycastHit hit;

        // TODO ora va bene, però quando inizia a cadere va lasciato cadere sino a quando non tocca terra, forse è a causa del Lerp
        float capsuleRadius = collider.bounds.size.x / 2;
        if(Physics.CapsuleCast(transform.position + new Vector3(0, collider.bounds.size.y  - capsuleRadius, -collider.bounds.size.z / 2 + capsuleRadius),
                               transform.position + new Vector3(0, collider.bounds.size.y - capsuleRadius, collider.bounds.size.z / 2 - capsuleRadius),
                               capsuleRadius,
                               Vector3.down,
                               out hit)) {
            float check = collider.bounds.size.y - capsuleRadius * 2;
            hitGround = hit.distance <= check;
           
        }

        Debug.Log(hitGround);

        // si calcola la distanza dal target
        distance = Vector3.Distance(origin, target.transform.position);

        Vector3 currentLerp;
		if(distance < triggerDistance) {
            currentLerp = Vector3.Lerp(transform.position, target.transform.position, speed * Time.deltaTime);
        } else {
            currentLerp = Vector3.Lerp(transform.position, origin, (speed / 2.0f) * Time.deltaTime);
        }

        if((!canFly && currentLerp.y > transform.position.y) || hitGround) {
            currentLerp.y = origin.y;
        }

        transform.position = currentLerp;
	}
}
