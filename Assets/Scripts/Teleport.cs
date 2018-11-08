using UnityEngine;

public class Teleport : MonoBehaviour {
    [SerializeField] public GameObject destination;
    [SerializeField] public bool reusable = false;

    private Transform character;
    private bool used = false;

    void Update() {
        if (character != null && (!used || reusable)) {
            character.position = new Vector3(destination.transform.position.x,
                                         destination.transform.position.y + character.position.y,
                                         destination.transform.position.z);
            //character.rotation = destination.transform.rotation; // TODO: reset camera
            character = null;
            used = true;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Player")) {
            this.character = other.gameObject.transform;
        }
    }
}
