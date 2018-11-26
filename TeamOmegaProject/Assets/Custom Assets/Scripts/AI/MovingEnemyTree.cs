using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingEnemyTree : BehaviorTree {
	// Use this for initialization
	void Start () {
		UnityEngine.AI.NavMeshAgent navmesh = GetComponent<UnityEngine.AI.NavMeshAgent>();
		if (navmesh == null)
			Debug.Log("NavMeshAgent could not be found");
		root = new Sequence(
							new ChangeColour(new UnityEngine.Color(0.1f, 0.4f, 0.1f)),
							new Alternator(
									new Sequence(
											new WaitForPlayer(5),
											new SendAlert(3)
									),
									new Patrol(navmesh),
									new Alert(25)
							),
							new ChangeColour(UnityEngine.Color.red),
							new LoopUntilFailure(
									new Timeout(10f,
											new ChasePlayer(navmesh, 2)
									)
							)
						);
	}
}
