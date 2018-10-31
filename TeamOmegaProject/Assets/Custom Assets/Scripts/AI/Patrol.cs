using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : BehaviorTreeNode {
	private bool hastarget = false;
	private float stuck = 1f;
	private UnityEngine.AI.NavMeshAgent navmesh;
	private Vector3 oldtransform = new Vector3(0,0,0);
	private Vector3 start = new Vector3(0,0,0);
	public Patrol(UnityEngine.AI.NavMeshAgent navmesh)
	{
		this.navmesh = navmesh;
	}
	public override int Act (BehaviorTree tree)
	{
		if(start == new Vector3(0,0,0))
		{
			start = tree.transform.position;
		}
		if(!navmesh.pathPending && (tree.transform.position - oldtransform).magnitude < 0.2f)
		{
			var t = Time.deltaTime;
			stuck= stuck - t;
		}
		else
		{
			stuck = 1f;
		}
		if(stuck < 0)
		{
			Vector3 position = tree.transform.position;
			navmesh.SetDestination(start);
			start = position;
			stuck = 1f;
		}
		if(!hastarget || (!navmesh.pathPending && (navmesh.remainingDistance < 0.1f)))
		{
			Vector3 goal = tree.transform.position + tree.transform.rotation * new Vector3(hastarget?1000:0, 0, hastarget?0:1000);
			UnityEngine.AI.NavMeshHit hit;
			UnityEngine.AI.NavMesh.Raycast(tree.transform.position, goal, out hit, navmesh.walkableMask);
			goal = hit.position;
			hastarget = true;
			navmesh.SetDestination(goal);
		}
		oldtransform = tree.transform.position;
		return -1;
	}
}
