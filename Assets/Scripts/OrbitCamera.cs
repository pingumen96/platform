using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitCamera : MonoBehaviour {
    private const float MAX_X_ROTATION = 52.0f;
    private const float MIN_X_ROTATION = -33.0f;

    [SerializeField] private Transform target;

    public float rotationSpeed = 1.5f;

    private float xRotation;
    private float yRotation;
    private Vector3 offset;

	// Use this for initialization
	void Start () {
        yRotation = transform.eulerAngles.y;
        xRotation = transform.eulerAngles.x;
        offset = target.position - transform.position;
	}
	
	// Update is called once per frame
	void LateUpdate () {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Mouse Y");

        if(horizontalInput != 0) {
            yRotation += horizontalInput * rotationSpeed;
        } else {
            yRotation += Input.GetAxis("Mouse X") * rotationSpeed * 3;
        }

        if(verticalInput != 0) {
            xRotation += -verticalInput * rotationSpeed * 3;
            
            // impostiamo la rotazione nel range voluto
            if(xRotation > MAX_X_ROTATION) { 
                xRotation = MAX_X_ROTATION;
            } else if(xRotation < MIN_X_ROTATION) {
                xRotation = MIN_X_ROTATION;
            }
        }

        Quaternion rotation = Quaternion.Euler(xRotation, yRotation, 0);
        transform.position = target.position - (rotation * offset);
        transform.LookAt(target);
    }
}
