using Godot;
using System;

public class Bumper : RigidBody2D
{
	[Export] private float strength = 1024f;
	

	public override void _Ready()
	{
		this.Connect("body_entered", this, "Fire");
	}

	private void Fire(Node node)
	{   
		if (node.GetType() == typeof(RigidBody2D))
		{
			RigidBody2D rigid = node as RigidBody2D;

			Vector2 rigidDir = GlobalPosition.DirectionTo(rigid.GlobalPosition).Normalized();
			rigid.ApplyImpulse(GlobalPosition, rigidDir * strength);
		}
	}
}
