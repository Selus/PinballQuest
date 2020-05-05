using Godot;
using System;

public class Spring : RigidBody2D
{
	[Export] private Vector2 direction;
	[Export] private float strength = 512f;
	

	public override void _Ready()
	{
		this.Connect("body_entered", this, "Fire");
	}

	private void Fire(Node node)
	{   
		if (node.GetType() == typeof(RigidBody2D))
		{
			RigidBody2D rigid = node as RigidBody2D;
			rigid.ApplyImpulse(GlobalPosition, direction * strength);
		}
	}
}
