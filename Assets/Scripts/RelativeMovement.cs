using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class RelativeMovement : MonoBehaviour {
    [SerializeField] private Transform target;
    public float rotationSpeed = 15.0f;
    public float moveSpeed = 6.0f;
    public float jumpSpeed = 15.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minFall = -1.5f;

    private float verticalSpeed;

    private CharacterController characterController;
    private ControllerColliderHit contact;
    private Animator animator;

    private void Start() {
        verticalSpeed = minFall;
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update () {

        Vector3 movement = Vector3.zero;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        if(horizontalInput != 0 || verticalInput != 0) {
            movement.x = horizontalInput * moveSpeed;
            movement.z = verticalInput * moveSpeed;

            movement = Vector3.ClampMagnitude(movement, moveSpeed);

            Quaternion tmp = target.rotation;
            target.eulerAngles = new Vector3(0, target.eulerAngles.y, 0);
            movement = target.TransformDirection(movement);
            target.rotation = tmp;

            //transform.rotation = Quaternion.LookRotation(movement);

            Quaternion direction = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Lerp(transform.rotation, direction, rotationSpeed * Time.deltaTime);
        }

        bool hitGround = false;
        RaycastHit hit;

        if(verticalSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit)) {
            float check = (characterController.height + characterController.radius) / 1.9f;
            hitGround = hit.distance <= check;
        }

        animator.SetFloat("Speed", movement.sqrMagnitude);

        if(hitGround) {
            if(Input.GetButtonDown("Jump")) {
                verticalSpeed = jumpSpeed;
            } else {
                //verticalSpeed = minFall;
                verticalSpeed = -2.0f;
                animator.SetBool("Jumping", false);
            }
        } else {
            verticalSpeed += gravity * 5 * Time.deltaTime;
            if(verticalSpeed < terminalVelocity) {
                verticalSpeed = terminalVelocity;
            }

            if(contact != null) {
                animator.SetBool("Jumping", true);
            }

            if(characterController.isGrounded) {
                if(Vector3.Dot(movement, contact.normal) < 0) {
                    movement = contact.normal * moveSpeed;
                } else {
                    movement += contact.normal * moveSpeed;
                }
            }
        }

        movement.y = verticalSpeed;


        movement *= Time.deltaTime;
        characterController.Move(movement);
	}

    private void OnControllerColliderHit(ControllerColliderHit hit) {
        contact = hit;
    }
}
