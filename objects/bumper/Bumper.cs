using Godot;
using System;

public class Bumper : RigidBody2D
{
	[Export] private float strength = 1024f;
	
	private AnimationPlayer anim;


	public override void _Ready()
	{
		this.Connect("body_entered", this, "Fire");

		anim = GetNode("anim") as AnimationPlayer;
		// anim.Play("dance");
	}


	private void Fire(Node node)
	{   
		if (node.GetType() == typeof(Ball))
		{
			RigidBody2D rigid = node as RigidBody2D;

			Vector2 rigidDir = GlobalPosition.DirectionTo(rigid.GlobalPosition).Normalized();
			rigid.LinearVelocity = rigidDir * strength;

			anim.Play("hit");
		}
	}
}
