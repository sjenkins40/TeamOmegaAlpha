using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StationaryEnemyTree : BehaviorTree {
	// Use this for initialization
	public Rigidbody projectile;
	public float speed;
	void Start () {
		root = new Sequence(
							new ChangeColour(new UnityEngine.Color(0.1f, 0.4f, 0.1f)),
							new WaitForPlayer(15),
							new SendAlert(3),
							new ChangeColour(UnityEngine.Color.red),
							new LoopUntilFailure(
									new Alternator(
											new IfNotSuccess(new WaitForPlayer(15)),
											new Attack(projectile, 0.5f, speed)
									)
							)
						);
	}
}
