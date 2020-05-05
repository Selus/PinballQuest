using Godot;
using System;

public class Enemy : RigidBody2D
{
	[Export] private float speed = 128f;
	[Export] private float distance = 256f;
	[Export] private int randomness = 64;
	[Export] private int health = 3;
	[Export] private string ball;

	private Vector2 startPos;
	private bool returning = false;
	private RandomNumberGenerator random;
	private Vector2 newVelocity;
	private Tween tween;

	public override void _Ready()
	{
		startPos = GlobalPosition;
		tween = GetNode("Tween") as Tween;

		this.Connect("body_entered", this, "Fly");

		random = new RandomNumberGenerator();
		Vector2 dir = new Vector2(random.RandfRange(-1f, 1f), random.RandfRange(-1f, 1f));
		LinearVelocity = dir * speed;
	}

	public override void _PhysicsProcess(float delta)
	{
		GD.Randomize();

		if (GlobalPosition.DistanceTo(startPos) > distance)
		{
			returning = true;
			newVelocity = GlobalPosition.DirectionTo(startPos).Normalized() * (speed * 2);
			tween.Stop(this);
			tween.InterpolateProperty(this, "linear_velocity", LinearVelocity, newVelocity, 1f);
			tween.Start();
		}

		if (GlobalPosition.DistanceTo(startPos) < distance/2 && returning == true)
		{
			returning = false;
			Change(true);
		}

		if (returning == false && random.RandiRange(0, randomness) == randomness)
		{
			Change(true, -1f, 1f);
		}
	}

	private void Fly(Node node)
	{
		Change();

		if (node.IsClass(ball))
		{
			health--;

			if (health <= 0)
			{
				this.RemoveAndSkip();
			}
		}
	}

	private void Change(bool rand = false, float randMin = -1f, float randMax = 1f)
	{
		float time;
		Vector2 start;

		if (rand)
		{
			Vector2 dirNew = Vector2.One;
			dirNew.x *= random.RandfRange(randMin, randMax);
			dirNew.y *= random.RandfRange(randMin, randMax);
			dirNew = dirNew.Normalized() * speed;
			newVelocity = dirNew;
			time = random.RandfRange(2f, 4f);
			start = LinearVelocity;
		} else {
			Vector2 dirNew = LinearVelocity;
			dirNew.x *= random.RandfRange(0.16f, 0.64f);
			dirNew.y *= random.RandfRange(0.16f, 0.64f);
			dirNew = dirNew.Normalized() * -1f * speed;
			newVelocity = dirNew;
			time = 0.25f;
			start = Vector2.Zero;
		}

		tween.Stop(this);
		tween.InterpolateProperty(this, "linear_velocity", start, newVelocity, time);
		tween.Start();
	}
}
