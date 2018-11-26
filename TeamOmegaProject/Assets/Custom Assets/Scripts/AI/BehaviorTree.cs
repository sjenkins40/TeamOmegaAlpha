using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorTree : MonoBehaviour {
	internal BehaviorTreeNode root = null;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		root.Act(this);
	}
	public class Timeout : BehaviorTreeNode
	{
		private float timeout;
		private float count;
		private BehaviorTreeNode child;
		public Timeout(float timeout, BehaviorTreeNode child)
		{
			this.timeout = timeout;
			this.count = timeout;
			this.child = child;
		}
		public override int Act(BehaviorTree tree)
		{
			count = count - Time.deltaTime;
			if(count < 0)
			{
				count = timeout;
				return 0;
			}
			return child.Act(tree);
		}
	}
	public class Sequence : BehaviorTreeNode
	{
		private int cur = 0;
		private BehaviorTreeNode[] children;
		public Sequence(params BehaviorTreeNode[] children)
		{
			this.children = children;
		}
		public override int Act(BehaviorTree tree)
		{
			int a = children[cur].Act(tree);
			if(a == 1)
			{
				cur++;
				if(cur == children.Length)
				{
					cur = 0;
					return 1;
				}
			}
			else if(a == 0)
			{
				cur = 0;
				return 0;
			}
			return -1;
		}
	}
	public class Selector : BehaviorTreeNode
	{
		private int cur = 0;
		private BehaviorTreeNode[] children;
		public Selector(params BehaviorTreeNode[] children)
		{
			this.children = children;
		}
		public override int Act(BehaviorTree tree)
		{
			int a = children[cur].Act(tree);
			if(a == 0)
			{
				cur++;
				if(cur == children.Length)
				{
					cur = 0;
					return 0;
				}
			}
			else if(a == 1)
			{
				cur = 0;
				return 1;
			}
			return -1;
		}
	}
	public class Alternator : BehaviorTreeNode
	{
		private int cur = 0;
		private BehaviorTreeNode[] children;
		public Alternator(params BehaviorTreeNode[] children)
		{
			this.children = children;
		}
		public override int Act(BehaviorTree tree)
		{
			int a = children[cur].Act(tree);
			cur++;
			if(cur == children.Length)
			{
				cur = 0;
			}
			return a;
		}
	}
	public class LoopUntilFailure : BehaviorTreeNode
	{
		private int cur = 0;
		private BehaviorTreeNode[] children;
		public LoopUntilFailure(params BehaviorTreeNode[] children)
		{
			this.children = children;
		}
		public override int Act(BehaviorTree tree)
		{
			int a = children[cur].Act(tree);
			if(a == 1)
			{
				cur++;
				if(cur == children.Length)
				{
					cur = 0;
				}
			}
			else if(a == 0)
			{
				cur = 0;
				return 0;
			}
			return -1;
		}
	}
	public class IfNotSuccess : BehaviorTreeNode
	{
		private BehaviorTreeNode child;
		public IfNotSuccess(BehaviorTreeNode child)
		{
			this.child = child;
		}
		public override int Act(BehaviorTree tree)
		{
			int a = child.Act(tree);
			if(a == 1)
			{
				return 1;
			}
			return 0;
		}
	}
}
