using Godot;
using System;
using System.Collections.Generic;

public class Main : Node2D
{
	private int points = 0;
	private BallCamera camera;
	private List<Ball> balls = new List<Ball>();
	private List<Checkpoint> checkpoints = new List<Checkpoint>();
	private Checkpoint currentCheckpoint;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		instance = this;
		camera = GetNode("BallCamera") as BallCamera;
		GetTree().Paused = true;
		var children = GetNode("Level_3").GetChildren();
		foreach (Node child in children)
		{
			if (child.GetType() == typeof(Checkpoint))
			{
				checkpoints.Add(child as Checkpoint);
			}
			if (child.GetType() == typeof(Ball))
			{
				balls.Add(child as Ball);
			}
		}
		// generateMap();
	}

	public override void _Process(float delta)
	{
		//ball tracker
		foreach(Ball ball in balls)
		{
		 	if(checkpoints.Count != 0)
		 	{
		 		for(int i = 0;i < checkpoints.Count ; i++)
				{
					var checkpoint = checkpoints[i];
					if(ball.Position.y < checkpoint.Position.y)
					{
						currentCheckpoint = checkpoint;
						checkpoints.Remove(checkpoint);
						camera.LimitBottom = (int)(checkpoint.Position.y);
					}
				}
		 	}
		}


		if (Input.IsActionJustPressed("ui_up"))
		{
			foreach(Ball ball in balls)
			{
				ball.ApplyImpulse(GlobalPosition, new Vector2(0,-1) * 1024);
			}
		}

		if (Input.IsActionJustPressed("ui_left"))
		{
			GetTree().CallGroup("FlipperLeft", "Activate");
		}
		if (Input.IsActionJustReleased("ui_left"))
		{
			GetTree().CallGroup("FlipperLeft", "Activate");
		}

		if (Input.IsActionJustPressed("ui_right"))
		{
			GetTree().CallGroup("FlipperRight", "Activate");
		}
		if (Input.IsActionJustReleased("ui_right"))
		{
			GetTree().CallGroup("FlipperRight", "Activate");
		}
		if (Input.IsActionJustReleased("pause"))
		{
			if(GetTree().Paused == true)
			{
				GetTree().Paused = false;
				hideGUI();
			} else {
				GetTree().Paused = true;
				showGUI();
			}
			
		}
	}
	private void showGUI()
	{
		(GetNode("HUDLayer/HUD/Menu") as Control).Visible = true;
	}
	private void hideGUI()
	{
		(GetNode("HUDLayer/HUD/Menu") as Control).Visible = false;
	}


	//api
	private static Main instance=null;
	static public Main GetInstance() 
	{
		return instance;
	}

	public void addPoints(int amount)
	{
		points =+ amount;
	}
	public int getPoints()
	{
		return points;
	}
	public void startGame()
	{
		GetTree().Paused = false;
		hideGUI();
	}
}