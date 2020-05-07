using Godot;
using System;

public class Booster : Area2D
{
	[Export] private float minVelocity = 2048f;
	[Export] private float multipler = 1;
	[Export] private bool forceCenter = false;

	private Vector2 direction;
	private Position2D directionPoint;
	private AnimationPlayer animation;
	

	public override void _Ready()
	{
		this.Connect("body_entered", this, "Fire");

		directionPoint = GetNode("Position2D") as Position2D;
		direction = GlobalPosition.DirectionTo(directionPoint.GlobalPosition);

		animation = GetNode("AnimationPlayer") as AnimationPlayer;
	}


	private void Fire(Node node)
	{   
		if (node.GetType() == typeof(Ball))
		{
			RigidBody2D rigid = node as RigidBody2D;
			
			float absVelocity = Mathf.Sqrt(rigid.LinearVelocity.x * rigid.LinearVelocity.x + rigid.LinearVelocity.y * rigid.LinearVelocity.y);

			if (forceCenter)
			{
				rigid.Sleeping = true;
				rigid.GlobalPosition = GlobalPosition;
			}

			if (absVelocity <= minVelocity)
			{
				absVelocity = minVelocity;    
			}

			rigid.LinearVelocity = direction * absVelocity * multipler;
			
			animation.Play("Color");

			Main.GetInstance().addPoints(1);
		}
	}
}
