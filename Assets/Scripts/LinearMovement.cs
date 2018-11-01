using System.Collections;
using UnityEngine;

public class LinearMovement : MonoBehaviour {
    [SerializeField] public Vector3 relativeMovement;
    [SerializeField] public float speed;
    [SerializeField] public float pauseSeconds;
    [SerializeField] public bool endRotation;

    private Vector3 origin;
    private Vector3 destination;
    private float startTime;
    private float distance;


    // Use this for initialization
    void Start() {
        origin = transform.position;
        destination = origin + relativeMovement;
        startTime = Time.time;
        distance = Vector3.Distance(origin, destination);
        StartCoroutine(Move());
    }

    IEnumerator Move() {
        while(Vector3.Distance(transform.position, destination) > 0.01f) {
            transform.position = Vector3.Lerp(origin, destination, ((Time.time - startTime) * speed) / distance);
            yield return null;
        }

        
        yield return new WaitForSeconds(pauseSeconds);

        if(endRotation) {
            transform.Rotate(new Vector3(0f, 180.0f, 0f), Space.Self);
        }

        Vector3 temp = origin;
        origin = destination;
        destination = temp;
        startTime = Time.time;

        StartCoroutine(Move());
    }
}
