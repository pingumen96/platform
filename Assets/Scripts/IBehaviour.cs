using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Character))]
public abstract class IBehaviour : MonoBehaviour {
    protected Character character;

    public virtual void Start() {
        character = GetComponent<Character>();
    }

    public virtual void MovementIntent() { }
}