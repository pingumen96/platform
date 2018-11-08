using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(IBehaviour))]
public class NonPlayableCharacter : Character {
    [SerializeField] private IBehaviour[] behaviours;

	// Use this for initialization
	new void Start () {
        base.Start();
        behaviours = GetComponents<IBehaviour>();
    }
	
	// Update is called once per frame
	void Update () {
        // si eseguono i comportamenti del NPC, uno ad uno nell'ordine in cui sono
        behaviours[0].MovementIntent();
	}
}
