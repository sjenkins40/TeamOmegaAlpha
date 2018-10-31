using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : BehaviorTreeNode {
	private Rigidbody projectile;
	private float firerate;
	private float countdown = -1;
	private float speed;
	public Attack(Rigidbody projectile, float firerate, float speed)
	{
		this.projectile = projectile;
		this.firerate = firerate;
		this.speed = speed;
	}
	public override int Act (BehaviorTree tree)
	{
		tree.transform.LookAt(player.transform);
		if(firerate > 0)
		{
			countdown -= Time.deltaTime;
			if(countdown < 0)
			{
				countdown = firerate;
				Vector3 direction = player.transform.position - tree.transform.position;
				direction = direction.normalized;
				Rigidbody t = UnityEngine.Object.Instantiate(projectile, tree.transform.position + direction, tree.transform.rotation);
				t.velocity = direction* speed;
				t.useGravity = false;
			}
		}
		return -1;
	}
}
