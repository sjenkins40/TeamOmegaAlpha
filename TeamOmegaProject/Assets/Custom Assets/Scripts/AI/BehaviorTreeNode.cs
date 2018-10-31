using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviorTreeNode {
	public static VelocityReporter player;
	//-1 - in progress
	//0 - failed
	//1 - succeeded
	public abstract int Act (BehaviorTree tree);
}
