using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : BehaviorTreeNode {
	private static float seen = 0;
	private float distance = 0;
	public Alert(float distance)
	{
		this.distance = distance;
	}
	public override int Act (BehaviorTree tree)
	{
		seen = seen - Time.deltaTime;
		if(seen > 0 && (player.transform.position - tree.transform.position).magnitude <= distance)
			return 1;
		return -1;
	}
	
	public static void see(float time)
	{
		seen = time;
	}
}
