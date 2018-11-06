using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : Character {
    [SerializeField] protected Transform cameraTransform;

    private Animator animator;

    new void Start() {
        base.Start();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {
        movement = Vector3.zero; // reset

        // update
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float runningInput = Input.GetAxis("Run");
        isJumping = Input.GetButton("Jump");

        // movement intent
        if (horizontalInput != 0 || verticalInput != 0) {
            // sulla base dell'input dell'utente, si stabilisce la direzione verso la quale si vuole andare
            MovementIntent(new Vector3(horizontalInput * moveSpeed * (1 + runningInput / 2f),
                                       movement.y,
                                       verticalInput * moveSpeed * (1 + runningInput / 2f)));
        }

        ReactToGravity();

        // update
        animator.SetFloat("Speed", movement.sqrMagnitude);
        animator.SetBool("Jumping", !hitGround || isJumping);

        Move();
    }

    protected override void MovementIntent(Vector3 direction) {
        base.MovementIntent(direction);
        movement = Vector3.ClampMagnitude(direction, moveSpeed * 1.5f);

        Quaternion tmp = cameraTransform.rotation;
        cameraTransform.eulerAngles = new Vector3(0, cameraTransform.eulerAngles.y, 0);
        movement = cameraTransform.TransformDirection(movement);
        cameraTransform.rotation = tmp;

        Quaternion characterRotation = Quaternion.LookRotation(movement);
        transform.rotation = Quaternion.Lerp(transform.rotation, characterRotation, rotationSpeed * Time.deltaTime);
    }

    /* Metodo responsabile per TUTTE le collisioni del giocatore */
    protected override void OnControllerColliderHit(ControllerColliderHit hit) {
        base.OnControllerColliderHit(hit);
        GameObject hitObject = hit.collider.gameObject;
        Collider collider = hit.collider;
        contact = hit;

        if (hitObject.CompareTag("Teleport")) {
            hitObject.GetComponent<Teleport>().OnCharacterEnter(transform);
        } else if (hitObject.CompareTag("Coin")) {
            // qui si gestirà il punteggio, ecc.
        } else if (hitObject.CompareTag("StompableEnemy")) {
            Vector3 center = new Vector3(collider.bounds.size.x / 2, 0.0f, collider.bounds.size.z / 2);
            if (Physics.CapsuleCast(hitObject.transform.position - center, hitObject.transform.position + center,
                                    collider.bounds.size.x / 2, Vector3.up)) {
                Destroy(hitObject);
            } else {
                /* si toglie un cuoricino al pg */
            }
        }
    }
}
