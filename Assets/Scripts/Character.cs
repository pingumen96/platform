using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(GenericAgent))]
public class Character : MonoBehaviour {
    [SerializeField] public float rotationSpeed = 15.0f;
    [SerializeField] public float jumpSpeed = 18.0f;
    [SerializeField] public float gravity = -9.8f;
    [SerializeField] public float terminalVelocity = -10.0f;
    [SerializeField] public float minFall = -1.5f;

    protected float verticalSpeed;
    protected bool isJumping = false;
    protected bool isHittingGround = false;

    protected CharacterController characterController;
    protected GenericAgent agent;
    protected ControllerColliderHit contact;
    protected Platform currentPlatform;

    // Use this for initialization
    protected virtual void Start() {
        verticalSpeed = minFall;
        characterController = GetComponent<CharacterController>();
        agent = GetComponent<GenericAgent>();
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
        Vector3 velocity = agent.velocity;

        if (!isHittingGround) {
            if(characterController.isGrounded) { // sono sopra qualcosa che non è terra, ad esempio, calpestando il procione
                if(Vector3.Dot(velocity, contact.normal) < 0) {
                    velocity = contact.normal * agent.maxSpeed;
                } else {
                    velocity += contact.normal * agent.maxSpeed;
                }
            }
        }

        velocity.y = verticalSpeed;
        velocity *= Time.deltaTime;
        characterController.Move(velocity);
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