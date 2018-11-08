using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Character : MonoBehaviour {
    [SerializeField] public float rotationSpeed = 15.0f;
    [SerializeField] public float moveSpeed = 6.0f;
    [SerializeField] public float jumpSpeed = 18.0f;
    [SerializeField] public float gravity = -9.8f;
    [SerializeField] public float terminalVelocity = -10.0f;
    [SerializeField] public float minFall = -1.5f;

    protected float verticalSpeed;
    protected Vector3 movement;
    protected bool isJumping = false;
    protected bool isHittingGround = false;

    protected CharacterController characterController;
    protected ControllerColliderHit contact;
    protected Platform currentPlatform;

    // Use this for initialization
    protected void Start () {
        verticalSpeed = minFall;
        characterController = GetComponent<CharacterController>();
    }
	
	// Update is called once per frame
	void Update () {
        MovementIntent();
        ReactToGravity();
        Move();
	}

    protected virtual void MovementIntent() {
        MovementIntent(Vector3.zero);
    }

    protected virtual void MovementIntent(Vector3 direction) {
        movement = Vector3.ClampMagnitude(direction, moveSpeed * 1.5f);
    }

    protected virtual void ReactToGravity() {
        isHittingGround = false;
        RaycastHit hit;

        if (verticalSpeed < 0 && Physics.Raycast(transform.position, Vector3.down, out hit)) {
            float check = (characterController.height + characterController.radius) / 1.9f;
            isHittingGround = hit.distance <= check;
        }

        if (isHittingGround) {
            /* innanzitutto verifichiamo se stiamo o meno sopra una piattaforma */
            if (currentPlatform != null) {
                if (characterController.collisionFlags != CollisionFlags.Below) {
                    currentPlatform.OnCharacterExit(transform);
                    currentPlatform = null;
                }
            }

            if (isJumping) {
                verticalSpeed = jumpSpeed;
            } else {
                //verticalSpeed = minFall;
                verticalSpeed = -2.0f;
            }
        } else {
            verticalSpeed += gravity * 5 * Time.deltaTime;
            if (verticalSpeed < terminalVelocity) {
                verticalSpeed = terminalVelocity;
            }
        }
    }

    protected virtual void Move() {
        if (!isHittingGround) {
            if (characterController.isGrounded) {
                if (Vector3.Dot(movement, contact.normal) < 0) {
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

    protected virtual void OnControllerColliderHit(ControllerColliderHit hit) {
        GameObject hitObject = hit.collider.gameObject;
        contact = hit;

        if (hitObject.CompareTag("Platform")) {
            currentPlatform = hitObject.GetComponent<Platform>();
            currentPlatform.OnCharacterEnter(transform);
        }
    }
}