using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour {
    private HashSet<Transform> characters;
    private Vector3 prevPosition;
    private Vector3 prevRotation;

    void Start () {
        characters = new HashSet<Transform>();
        prevPosition = transform.position;
        prevRotation = transform.eulerAngles;
    }
	
	void Update () {
        Vector3 currentPosition = transform.position;
        Vector3 currentRotation = transform.eulerAngles;
        foreach (Transform t in characters) {
            t.position += currentPosition - prevPosition;
            t.eulerAngles += currentRotation - prevRotation; // testare piattaforme con rotazioni
        }
        prevPosition = currentPosition;
        prevRotation = currentRotation;
    }

    public void OnCharacterEnter(Transform character) {
        characters.Add(character);
    }

    public void OnCharacterExit(Transform character) {
        characters.Remove(character);
    }
}
