using Godot;
using System;

public class Camera2D : Godot.Camera2D
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
	// if (Input.IsActionJustPressed("ui_left"))
	// 	{
	// 		Position = new Vector2(Position.x-100,Position.y);
	// 	}
	// 	if (Input.IsActionJustPressed("ui_right"))
	// 	{
	// 		Position = new Vector2(Position.x+100,Position.y);
	// 	}
	// 	if (Input.IsActionJustPressed("ui_down"))
	// 	{
	// 		Position = new Vector2(Position.x,Position.y+100);
	// 	}
	// 	if (Input.IsActionJustPressed("ui_up"))
	// 	{
	// 		Position = new Vector2(Position.x,Position.y-100);
	// 	}
	RigidBody2D ball = GetNode("/root/Main/Ball") as RigidBody2D;
	Position = new Vector2(ball.Position.x,ball.Position.y);
  }
}
