using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VelocityReporter : MonoBehaviour {
private Vector3 prevPos;
	public Vector3 velocity;

	// Use this for initialization
	void Start () {
		BehaviorTreeNode.player = this;
		prevPos = this.transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		velocity = (this.transform.position - prevPos) / Time.deltaTime;
		prevPos = this.transform.position;
	}
}
