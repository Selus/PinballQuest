using Godot;
using System;

public class Bumpable : RigidBody2D
{
	[Export] private float strength = 1024f;
	
	private AnimationPlayer anim;


	public override void _Ready()
	{
		this.Connect("body_entered", this, "Fire");
		SetPhysicsProcess(true);
	}


	// public override void _IntegrateForces(Physics2DDirectBodyState state)
	// {
	// 	if (state.GetContactCount() > 0)
	// 	{
	// 		GD.Print(state.GetContactLocalPosition(0));
	// 		GD.Print(state.GetContactLocalPosition(1));
	// 	}
	// }


	private void Fire(Node node)
	{   
		if (node.GetType() == typeof(Ball))
		{
			RigidBody2D rigid = node as RigidBody2D;

			Vector2 rigidDir = GlobalPosition.DirectionTo(rigid.GlobalPosition).Normalized();
			rigid.LinearVelocity = rigidDir * strength;
		}
	}
}
