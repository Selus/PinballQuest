using Godot;
using System;

public class Stars : Area2D
{
	AnimationPlayer anim;
	Polygon2D carrot;
	Particles2D particles;
	Timer timer;


	public override void _Ready()
	{
		anim = GetNode("AnimationPlayer") as AnimationPlayer;
		anim.Play("scale");
		carrot = GetNode("carrot") as Polygon2D;
		particles = GetNode("Particles2D") as Particles2D;
		timer = GetNode("Timer") as Timer;

		Connect("body_entered", this, "BodyEntered");
		timer.Connect("timeout", this, "Timeout");
	}

	private void BodyEntered(Node node)
	{
		if (node.GetType() == typeof(Ball))
		{
			carrot.Visible = false;
			Main.GetInstance().addPoints(5);
			particles.Emitting = true;
			timer.Start();
			Monitorable = false;
			Monitoring = false;
		}
	}

	private void Timeout()
	{
		this.GetParent().RemoveChild(this);
		this.QueueFree();
	}
}
