using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIAgent))]
public class AgentBehaviour : MonoBehaviour {
    public GameObject target;
    protected AIAgent agent;

    public virtual void Awake() {
        agent = gameObject.GetComponent<AIAgent>();
    }

    public virtual void Update() {
        agent.SetSteering(GetSteering());
    }

    public virtual Steering GetSteering() {
        return new Steering();
    }

}
