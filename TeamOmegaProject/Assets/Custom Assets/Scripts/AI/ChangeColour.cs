using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeColour : BehaviorTreeNode {
	private UnityEngine.Color color;
	public ChangeColour(UnityEngine.Color color)
	{
		this.color = color;
	}
	public override int Act (BehaviorTree tree)
	{
		Transform[] tempTransforms = tree.GetComponentsInChildren<Transform>();
		foreach(Transform child in tempTransforms)
		{
			if(child.gameObject.tag == "Eye")
			{
				UnityEngine.Renderer rend = child.GetComponent<Renderer>();
				rend.material.color = color;
			}
		}
		return 1;
	}
}
