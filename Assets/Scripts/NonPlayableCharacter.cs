using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NonPlayableCharacter : Character {
    [SerializeField] private IBehaviour[] behaviours;

	// Use this for initialization
	new void Start () {
        base.Start();
        behaviours = GetComponents<IBehaviour>();
    }

    protected override void MovementIntent() {
        base.MovementIntent();
        foreach(IBehaviour b in behaviours) {
            // si eseguono i comportamenti del NPC, uno ad uno nell'ordine in cui sono
            b.MovementIntent();
        }
    }
}
