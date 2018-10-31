using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinearMovement : MonoBehaviour {
    [SerializeField] public Vector3 relativeMovement;
    [SerializeField] public float speed;
    [SerializeField] public float pauseSeconds;
    private Vector3 origin;
    private Vector3 destination;
    private float startTime;
    private float distance;


    // Use this for initialization
    public void Start() {
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

        Vector3 temp = origin;
        origin = destination;
        destination = temp;
        startTime = Time.time;

        StartCoroutine(Move());
    }
}
