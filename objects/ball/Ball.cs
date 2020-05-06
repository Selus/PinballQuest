using Godot;
using System;

public class Ball : RigidBody2D
{
	[Export] public float velocityMax = 1024f;
	[Export] public float angularVelocityMax = 256f;

	[Export] AudioStream knock_0;
	[Export] AudioStream knock_1;
	[Export] AudioStream knock_2;
	[Export] AudioStream knock_3;
	[Export] AudioStream powerup_0;
	[Export] AudioStream powerup_1;

	AudioStreamPlayer2D audioKnock;
	Timer timer;
	RandomNumberGenerator random;
	Node lastTouch;


	public override void _Ready()
	{
		this.Connect("body_entered", this, "_BodyEntered");

		audioKnock = GetNode("AudioKnock") as AudioStreamPlayer2D;
		timer = GetNode("Timer") as Timer;

		random = new RandomNumberGenerator();
	}

	private void _BodyEntered(Node node)
	{
		GD.Randomize();

		if (timer.TimeLeft == 0 && node.GetType() != typeof(Stars))
		{   
			if (LinearVelocity.x > velocityMax * 0.75 || LinearVelocity.y > velocityMax * 0.75)
			{
				audioKnock.Stream = knock_0;
			}
			else if (LinearVelocity.x > velocityMax * 0.50 || LinearVelocity.y > velocityMax * 0.50)
			{
				audioKnock.Stream = knock_1;
			}
			else if (LinearVelocity.x > velocityMax * 0.25 || LinearVelocity.y > velocityMax * 0.25){
				if (random.RandiRange(0, 1) == 0)
				{
					audioKnock.Stream = knock_2;
				} else {
					audioKnock.Stream = knock_3;
				}
			}
			audioKnock.PitchScale = random.RandfRange(0.75f, 1.25f);
			audioKnock.Play();
			timer.Start();
		}

		if (node.GetType() == typeof(Stars))
		{
			if (random.RandiRange(0, 1) == 0)
			{
				audioKnock.Stream = powerup_0;
			} else {
				audioKnock.Stream = powerup_1;
			}
			audioKnock.PitchScale = random.RandfRange(0.75f, 1.25f);
			audioKnock.Play();
		}
	}


	public override void _PhysicsProcess(float delta)
	{
		LinearVelocity = LinearVelocity.Clamped(velocityMax);
		AngularVelocity = Mathf.Clamp(AngularVelocity, -angularVelocityMax, angularVelocityMax);
	}
}
