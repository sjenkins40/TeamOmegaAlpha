using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alarm : BehaviorTree {
	void Start () {
		root = new Sequence(
							new ChangeColour(new UnityEngine.Color(0.1f, 0.4f, 0.1f)),
							new WaitForPlayer(15),
							new SendAlert(10),
							new ChangeColour(UnityEngine.Color.red),
							new LoopUntilFailure(
								new IfNotSuccess(new WaitForPlayer(15))
							)
						);
	}
}
