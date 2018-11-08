using System.Collections.Generic;
using UnityEngine;

/*
 VANNO GESTITE INTERFACCIA + CLASSI VARIE IN MANIERA TALE CHE I PARAMETRI POSSANO ESSERE PASSATI DALL'INSPECTOR
 https://answers.unity.com/questions/46210/how-to-expose-a-field-of-type-interface-in-the-ins.html
     
     */


public class Platform : MonoBehaviour {
    private IMovement movement;

    // serve per interfacciarsi col motore di Unity
    private HashSet<Transform> characters;
    private Vector3 prevPosition;
    private Vector3 prevRotation;

    void Start () {
        movement = GetComponent<IMovement>();
        characters = new HashSet<Transform>();
        prevPosition = transform.position;
        prevRotation = transform.eulerAngles;
        movement.Init();
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
