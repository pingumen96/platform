using System.Collections;
using UnityEngine;

public class LinearMovement : IMovement {
    [SerializeField] public Vector3 relativeMovement;
    [SerializeField] public float speed;
    [SerializeField] public float pauseSeconds;
    [SerializeField] public bool endRotation;

    private Vector3 origin;
    private Vector3 destination;
    private float startTime;
    private float distance;
    private new Rigidbody rigidbody;


    // Use this for initialization
    public override void Init() {
        rigidbody = GetComponent<Rigidbody>();
        origin = transform.position;
        destination = origin + relativeMovement;
        startTime = Time.time;
        distance = Vector3.Distance(origin, destination);
        StartCoroutine(Move());
    }

    public override IEnumerator Move() {
        while(Vector3.Distance(transform.position, destination) > 0.01f) {
            rigidbody.MovePosition(Vector3.Lerp(origin, destination, ((Time.time - startTime) * speed) / distance));
            yield return null;
        }

        
        yield return new WaitForSeconds(pauseSeconds);

        if(endRotation) {
            rigidbody.MoveRotation(Quaternion.Euler(0f, 180.0f, 0f));
        }

        Vector3 temp = origin;
        origin = destination;
        destination = temp;
        startTime = Time.time;

        StartCoroutine(Move());
    }
}
