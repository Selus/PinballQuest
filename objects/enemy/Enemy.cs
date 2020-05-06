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

	private AnimationPlayer anim;
	private AudioStreamPlayer2D audioDead;
	private Sprite spriteBee;
	private Timer timer;


	public override void _Ready()
	{
		startPos = GlobalPosition;
		tween = GetNode("Tween") as Tween;
		timer = GetNode("Timer") as Timer;
		audioDead = GetNode("AudioStreamPlayer2D") as AudioStreamPlayer2D;

		this.Connect("body_entered", this, "Fly");
		timer.Connect("timeout", this, "Timeout");

		random = new RandomNumberGenerator();
		System.Random seed = new System.Random();
		random.Seed = this.GetInstanceId() + (ulong)System.DateTime.Now.Second;

		Vector2 dir = new Vector2(random.RandfRange(-1f, 1f), random.RandfRange(-1f, 1f));
		LinearVelocity = dir * speed;

		anim = GetNode("AnimationPlayer") as AnimationPlayer;
		anim.Play("Wings");

		spriteBee = GetNode("Bee") as Sprite;
	}


	public override void _PhysicsProcess(float delta)
	{
		if (health > 0)
		{
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
	}


	private void Fly(Node node)
	{
		Change();

		if (node.GetType() == typeof(Ball) && health > 0 && timer.TimeLeft <= 0)
		{
			health--;
			timer.Start();

			spriteBee.Modulate -= Color.Color8(0, 50, 100, 0);

			if (health <= 0)
			{
				spriteBee.Modulate = Color.Color8(200, 25, 25, 200);
				CollisionMask = 0;
				CollisionLayer = 0;
				LinearVelocity = Vector2.Zero;
				GravityScale = 1;
				ApplyCentralImpulse(Vector2.Down * speed);
				ApplyTorqueImpulse(speed);
				timer.WaitTime = 10;
				timer.Start();
				anim.Stop();
				audioDead.Play();
			}
		}
	}


	private void Timeout()
	{
		if (health <= 0)
		{
			this.RemoveAndSkip();
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
