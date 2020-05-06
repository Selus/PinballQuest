using Godot;
using System;

public class Spring : RigidBody2D
{
	// [Export] private Vector2 direction;
	[Export] private float strength = 512f;

	private Vector2 direction;
	private Position2D directionPoint;
	private AnimationPlayer animation;
	

	public override void _Ready()
	{
		this.Connect("body_entered", this, "Fire");
		directionPoint = GetNode("Position2D") as Position2D;
		direction = GlobalPosition.DirectionTo(directionPoint.GlobalPosition);
		animation = GetNode("Sprite/AnimationPlayer") as AnimationPlayer;
	}

	private void Fire(Node node)
	{   
		if (node.GetType() == typeof(Ball))
		{
			RigidBody2D rigid = node as RigidBody2D;
			rigid.ApplyImpulse(GlobalPosition, direction * strength);
			animation.Play("Spring");
		}
	}
}
