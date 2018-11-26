using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : BehaviorTreeNode {
	private Rigidbody projectile;
	private float firerate;
	private float countdown = -1;
	private float speed;
	private bool lookat;
	public Attack(Rigidbody projectile, float firerate, float speed, bool lookat)
	{
		this.projectile = projectile;
		this.firerate = firerate;
		this.speed = speed;
		this.lookat = lookat;
	}
	public override int Act (BehaviorTree tree)
	{
		if(lookat)
			tree.transform.LookAt(player.transform);
		if(firerate > 0)
		{
			countdown -= Time.deltaTime;
			if(countdown < 0)
			{
				countdown = firerate;
				Vector3 direction = player.transform.position - tree.transform.position + new Vector3(0,1,0);
				direction = direction.normalized;
				Rigidbody t = UnityEngine.Object.Instantiate(projectile, tree.transform.position + direction, tree.transform.rotation);
				t.velocity = direction* speed;
				t.useGravity = false;
				Object.Destroy(t, 30);
			}
		}
		return -1;
	}
}
