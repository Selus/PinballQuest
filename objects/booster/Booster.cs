using Godot;
using System;

public class Booster : Area2D
{
	[Export] private float strength = 2048f;
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
            GD.Print("Booster");
			RigidBody2D rigid = node as RigidBody2D;
            
            float absVelocity = Mathf.Sqrt(rigid.LinearVelocity.x * rigid.LinearVelocity.x + rigid.LinearVelocity.y * rigid.LinearVelocity.y);

            if (forceCenter)
            {
                rigid.GlobalPosition = GlobalPosition;
            }

            if (absVelocity <= strength)
            {
                absVelocity = strength;    
            }

            rigid.LinearVelocity = direction * absVelocity;
            
			animation.Play("Color");
		}
	}
}
