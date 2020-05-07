using Godot;
using System;

public class BallCamera : Godot.Camera2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
	}

	public void dezoom()
	{
		var tween = (Tween)GetNode("TweenZoom");
		tween.InterpolateProperty(this, "Zoom",	this.Zoom, new Vector2(1,1), 0.4f,Tween.TransitionType.Quad,Tween.EaseType.Out);
		tween.Start();
	}

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
	var balls =  Main.GetInstance().getBalls();
	var starter =  Main.GetInstance().getStarter();
	if(starter != null)
	{
		Vector2 newPosition= new Vector2(starter.Position.x,starter.Position.y);
		this.Zoom = new Vector2(0.65f,0.65f);
		var tween = (Tween)GetNode("Tween");
		tween.RemoveAll();
		tween.InterpolateProperty(this, "Position",	this.Position, newPosition, 0.4f,Tween.TransitionType.Quad,Tween.EaseType.Out);
		tween.Start();
	}
	else 
	{
		Ball lastBall = null;
		Vector2 newPosition = new Vector2(0,0);
		foreach(Ball ball in balls)
		{	
			if(lastBall == null)
			{
				newPosition = new Vector2(this.Position.x,ball.Position.y);
			}
			else if(ball.Position.y > lastBall.Position.y)
			{
				newPosition = new Vector2(this.Position.x,ball.Position.y);
			}
			lastBall = ball;
		}
		var tween = (Tween)GetNode("Tween");
		tween.RemoveAll();
		tween.InterpolateProperty(this, "Position",	this.Position, newPosition, 0.3f,Tween.TransitionType.Quad,Tween.EaseType.Out);
		tween.Start();
	}
  }
}
