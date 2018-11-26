using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendAlert : BehaviorTreeNode {
	private float time = 0;
	public SendAlert(float time)
	{
		this.time = time;
	}
	public override int Act (BehaviorTree tree)
	{
		Alert.see(time);
		return 1;
	}
}
