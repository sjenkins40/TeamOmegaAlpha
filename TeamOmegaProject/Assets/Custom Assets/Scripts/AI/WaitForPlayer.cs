using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitForPlayer : BehaviorTreeNode {
	private float distance = 0;
	public WaitForPlayer(float distance)
	{
		this.distance = distance;
	}
	public override int Act (BehaviorTree tree)
	{
		if((player.transform.position - tree.transform.position).magnitude <= distance)
			return 1;
		return -1;
	}
}
