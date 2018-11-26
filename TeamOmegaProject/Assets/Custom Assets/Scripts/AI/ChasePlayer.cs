using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChasePlayer : BehaviorTreeNode {
	private float counter = 0;
	private float distance;
	private UnityEngine.AI.NavMeshAgent navmesh;
	public ChasePlayer(UnityEngine.AI.NavMeshAgent navmesh, float distance)
	{
		this.navmesh = navmesh;
		this.distance = distance;
	}
	public override int Act (BehaviorTree tree)
	{
		if((player.transform.position - tree.transform.position).magnitude < distance)
			return 1;
		if(!navmesh.pathPending)
		{
			counter = counter - Time.deltaTime;
			if(counter <= 0)
			{
				counter = 0.3f;
				Vector3 goal = player.transform.position + player.velocity * predictTime(tree);
				UnityEngine.AI.NavMeshHit hit;
				if(UnityEngine.AI.NavMesh.Raycast(player.transform.position, goal, out hit, 0))
					goal = hit.position;
				navmesh.SetDestination(goal);
			}
		}
		return -1;
	}
	private float predictTime(BehaviorTree tree)
	{
		return ((player.transform.position - tree.transform.position).magnitude) / (navmesh.speed);
	}
}
