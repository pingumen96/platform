using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(Agent))]
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
    protected Agent agent;
    protected ControllerColliderHit contact;
    protected Platform currentPlatform;

    // Use this for initialization
    protected virtual void Start() {
        verticalSpeed = minFall;
        characterController = GetComponent<CharacterController>();
        agent = GetComponent<Agent>();
    }

    // Update is called once per frame
    protected virtual void Update() {
        ReactToGravity();
        Animate();
        Move();
    }

    protected virtual void ReactToGravity() {
        isHittingGround = false;

        if(verticalSpeed < 0) {
            RaycastHit[] hits = Physics.RaycastAll(transform.position, Vector3.down, (characterController.height + characterController.radius) / 1.9f);

            for(short i = 0; i < hits.GetLength(0) && !isHittingGround; i++) {
                isHittingGround = !hits[i].collider.isTrigger;
            }
        }

        if(isHittingGround) {
            /* innanzitutto verifichiamo se stiamo o meno sopra una piattaforma */
            if(currentPlatform != null) {
                if(characterController.collisionFlags != CollisionFlags.Below) {
                    currentPlatform.OnCharacterExit(transform);
                    currentPlatform = null;
                }
            }

            if(isJumping) {
                verticalSpeed = jumpSpeed;
            } else {
                //verticalSpeed = minFall;
                verticalSpeed = -2.0f;
            }
        } else {
            verticalSpeed += gravity * 5 * Time.deltaTime;
            if(verticalSpeed < terminalVelocity) {
                verticalSpeed = terminalVelocity;
            }
        }
    }

    protected virtual void Animate() {

    }

    protected virtual void Move() {
        if(!isHittingGround) {
            if(characterController.isGrounded) { // sono sopra qualcosa che non è terra, ad esempio, calpestando il procione
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

    protected virtual void OnControllerColliderHit(ControllerColliderHit hit) {
        GameObject hitObject = hit.collider.gameObject;
        contact = hit;

        if(hitObject.CompareTag("Platform")) {
            currentPlatform = hitObject.GetComponent<Platform>();
            currentPlatform.OnCharacterEnter(transform);
        }
    }
}