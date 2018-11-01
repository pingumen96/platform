using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {
    [SerializeField] private Transform target;
    public float rotationSpeed = 15.0f;
    public float moveSpeed = 6.0f;
    public float jumpSpeed = 18.0f;
    public float gravity = -9.8f;
    public float terminalVelocity = -10.0f;
    public float minFall = -1.5f;

    private float verticalSpeed;

    private CharacterController characterController;
    private ControllerColliderHit contact;
    private Animator animator;
    private Platform currentPlatform;

    private void Start() {
        verticalSpeed = minFall;
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        Vector3 movement = Vector3.zero;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float runningInput = Input.GetAxis("Run");

        if (horizontalInput != 0 || verticalInput != 0) {
            movement.x = horizontalInput * moveSpeed * (1 + runningInput / 2f);
            movement.z = verticalInput * moveSpeed * (1 + runningInput / 2f);

            movement = Vector3.ClampMagnitude(movement, moveSpeed * 1.5f);

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
            /* innanzitutto verifichiamo se stiamo o meno sopra una piattaforma */
            if(currentPlatform != null) {
                if(characterController.collisionFlags != CollisionFlags.Below) {
                    currentPlatform.OnCharacterExit(transform);
                    currentPlatform = null;
                }
            }


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



    /* Metodo responsabile per TUTTE le collisioni del giocatore */
    private void OnControllerColliderHit(ControllerColliderHit hit) {
        GameObject gameObject = hit.collider.gameObject;
        Collider collider = hit.collider;
        contact = hit;

        if (gameObject.CompareTag("Platform")) {
            currentPlatform = gameObject.GetComponent<Platform>();
            currentPlatform.OnCharacterEnter(transform);
        } else if (gameObject.CompareTag("Teleport")) {
            gameObject.GetComponent<Teleport>().OnCharacterEnter(transform);
        } else if (gameObject.CompareTag("Coin")) {
            // qui si gestirà il punteggio, ecc.
        } else if (gameObject.CompareTag("StompableEnemy")) {
            Debug.Log("collisione");
            if(Physics.CapsuleCast(gameObject.transform.position - new Vector3(collider.bounds.size.x / 2, 0.0f, collider.bounds.size.z / 2),
                                   gameObject.transform.position + new Vector3(collider.bounds.size.x / 2, 0.0f, collider.bounds.size.z / 2),
                                   collider.bounds.size.x / 2,
                                   Vector3.up)) {
                Destroy(gameObject);
            } else {
                /* si toglie un cuoricino al pg */
            }
        }
    }
}
