using Godot;
using System;
using System.Collections.Generic;

public class Main : Node2D
{
	private int points = 0;
	private int lives = 1;
	private BallCamera camera;
	private List<Ball> balls = new List<Ball>();
	private List<Checkpoint> checkpoints = new List<Checkpoint>();
	private List<Flipper> flippers = new List<Flipper>();
	private Checkpoint currentCheckpoint;
	private Node mainLevel;
	private AudioStreamPlayer2D audioPoint;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		instance = this;
		mainLevel = GetNode("Level_3");
		GetTree().Paused = true;
		camera = GetNode("BallCamera") as BallCamera;
		audioPoint = camera.GetNode("AudioPoint") as AudioStreamPlayer2D;
	}

	public override void _Process(float delta)
	{
		//ball tracker
		for(int i = 0;i < balls.Count ; i++)
		{
			var ball = balls[i];
		 	if(checkpoints.Count != 0)
		 	{
		 		for(int j = 0;j < checkpoints.Count ; j++)
				{
					var checkpoint = checkpoints[j];
					if(ball.Position.y < checkpoint.Position.y-200)
					{
						currentCheckpoint = checkpoint;
						checkpoints.Remove(checkpoint);
						camera.LimitBottom = (int)(checkpoint.Position.y);
					}
				}
		 	}
			if(flippers.Count != 0)
		 	{
		 		for(int j = 0; j < flippers.Count ; j++)
				{
					var flipper = flippers[j];
					if(flipper.folded == true)
					{
						if(ball.Position.y < flipper.Position.y)
						{
							flipper.Unfold();
						}
					}
				}
			}

			if(ball.Position.y > currentCheckpoint.Position.y)
			{
				if(balls.Count > 1)
				{
					balls.Remove(ball);
					ball.QueueFree();
				} 
				else if(balls.Count == 1)
				{
					balls.Remove(ball);
					gameOver();
				}
			}
		}


//input manager
		if (Input.IsActionJustPressed("ui_up"))
		{
			for(int i = 0;i < balls.Count ; i++)
			{
				var ball = balls[i];
				ball.ApplyImpulse(GlobalPosition, new Vector2(0,-1) * 1024);
			}
		}
		if (Input.IsActionJustPressed("restart"))
		{
			restartGame();
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
				resume();
			} else {
				pause();
			}
			
		}
	}

	//--------------------------------------api------------------------------------
	private static Main instance=null;
	static public Main GetInstance() 
	{
		return instance;
	}

	public void addPoints(int amount)
	{
		audioPoint.Stop(); // fixing non playing streams
		audioPoint.PitchScale = Mathf.Clamp(0.75f + amount/5, 0.75f, 2f);
		audioPoint.Play();
		points += amount;

	}
	public int getPoints()
	{
		return points;
	}
	public void MULTIBALL(Ball ball)
	{
		var newBall = (Ball)ball.Duplicate();
		balls.Add(newBall);
		newBall.ApplyImpulse(GlobalPosition, new Vector2(0,-1) * 1024);
		AddChild(newBall);
	}
	public IList<Ball> getBalls()
	{
		return balls.AsReadOnly();
	}
	public void startGame()
	{
		restartGame();
	}
	public void pause()
	{
		GetTree().Paused = true;
		HUD.GetInstance().showPauseMenu();
	}
	public void resume()
	{
		GetTree().Paused = false;
		HUD.GetInstance().hidePauseMenu();
	}
	public void gameOver()
	{
		GetTree().Paused = true;
		HUD.GetInstance().showGameOver();
	}
	public void restartGame()
	{
		points = 0;
		lives = 1;
		balls = new List<Ball>();
		checkpoints = new List<Checkpoint>();
		flippers = new List<Flipper>();
		currentCheckpoint = null;
		mainLevel.QueueFree();
		var scene = (PackedScene)ResourceLoader.Load("res://levels/Level_JP_3.tscn");
		var node = (Node2D)scene.Instance();
		camera.LimitBottom = 1000000;
		AddChild(node);
		mainLevel = node;
		var childrenCount = node.GetChildCount();
		if(childrenCount != 0)
		{
			for(int i = 0;i < childrenCount ; i++)
			{
				var child = node.GetChild(i);
				if (child.GetType() == typeof(Checkpoint))
				{
					checkpoints.Add(child as Checkpoint);
				}
				if (child.GetType() == typeof(Ball))
				{
					balls.Add((Ball)child);
				}
				if (child.GetType() == typeof(Flipper))
				{
					flippers.Add(child as Flipper);
				}
			}
		}
		GetTree().Paused = false;
		HUD.GetInstance().hideAll();
	}
}
