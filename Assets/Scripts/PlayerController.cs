using UnityEngine;

public class PlayerController : Character {
    [SerializeField] protected Transform cameraTransform;

    private Animator animator;

    protected override void Start() {
        base.Start();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void Update() {
        agent.velocity = Vector3.zero; // reset

        // update
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        float runningInput = Input.GetAxis("Run");
        isJumping = Input.GetButtonDown("Jump");

        // movement intent
        if (horizontalInput != 0 || verticalInput != 0) {
            // sulla base dell'input dell'utente, si stabilisce la direzione verso la quale si vuole andare
            agent.velocity.x = horizontalInput * agent.maxSpeed * (1 + runningInput / 2f);
            agent.velocity.z = verticalInput * agent.maxSpeed * (1 + runningInput / 2f);
            agent.velocity = Vector3.ClampMagnitude(agent.velocity, agent.maxSpeed * 1.5f);

            Quaternion tmp = cameraTransform.rotation;
            cameraTransform.eulerAngles = new Vector3(0, cameraTransform.eulerAngles.y, 0);
            agent.velocity = cameraTransform.TransformDirection(agent.velocity);
            cameraTransform.rotation = tmp;

            Quaternion characterRotation = Quaternion.LookRotation(agent.velocity);
            transform.rotation = Quaternion.Lerp(transform.rotation, characterRotation, rotationSpeed * Time.deltaTime);
        }

        base.Update();
    }

    protected override void Animate() {
        animator.SetFloat("Speed", agent.velocity.sqrMagnitude);
        animator.SetBool("Jumping", !isHittingGround || isJumping);
    }

    /* Metodo responsabile per TUTTE le collisioni del giocatore */
    protected override void OnControllerColliderHit(ControllerColliderHit hit) {
        base.OnControllerColliderHit(hit);
        GameObject hitObject = hit.collider.gameObject;
        Collider collider = hit.collider;
        //contact = hit;

        if (hitObject.CompareTag("Coin")/* || hitObject.CompareTag("Heart")*/) {
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
