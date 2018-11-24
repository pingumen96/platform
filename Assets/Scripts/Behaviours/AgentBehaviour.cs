using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AIAgent))]
public class AgentBehaviour : MonoBehaviour {
    public float weight = 1.0f;
    public GameObject target;
    protected AIAgent agent;

    public virtual void Awake() {
        agent = gameObject.GetComponent<AIAgent>();
    }

    public virtual void Update() {
        agent.SetSteering(GetSteering(), weight);
    }

    public virtual Steering GetSteering() {
        return new Steering();
    }

}
