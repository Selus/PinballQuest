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

  // Called every frame. 'delta' is the elapsed time since the previous frame.
  public override void _Process(float delta)
  {
	RigidBody2D ball = GetNode("/root/Main/Level_3/Ball") as RigidBody2D;
	if(ball != null) {
		Position = new Vector2(ball.Position.x,ball.Position.y);
	}
  }
}
