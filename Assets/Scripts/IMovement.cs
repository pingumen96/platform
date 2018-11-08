using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public abstract class IMovement : MonoBehaviour {
    public virtual void Init() { }
    public virtual IEnumerator Move() { return null; }
}
