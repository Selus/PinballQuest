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
	private AudioStreamPlayer2D audioMusic;
	private HamsterStarter starterHamster;
	private PackedScene sparkle = (PackedScene)ResourceLoader.Load("res://objects/SuperSparkle/SuperSparkle.tscn");
	bool audiMusicPlaying = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		instance = this;
		mainLevel = GetNode("SuperLevel");
		GD.Print(mainLevel);
		GetTree().Paused = true;
		camera = GetNode("BallCamera") as BallCamera;

		audioPoint = camera.GetNode("AudioPoint") as AudioStreamPlayer2D;
		audioMusic = camera.GetNode("AudioMusic") as AudioStreamPlayer2D;

		audioMusic.Play();
	}

	public override void _Process(float delta)
	{
		// if (!audiMusicPlaying)
		// {
		// 	audioMusic.Play();
		// 	audiMusicPlaying = true;
		// }

		//ball tracker
		for(int i = 0;i < balls.Count ; i++)
		{
			var ball = balls[i];
		 	if(checkpoints.Count != 0)
		 	{
		 		for(int j = 0;j < checkpoints.Count ; j++)
				{
					var checkpoint = checkpoints[j];
					if(ball.GlobalPosition.y < checkpoint.GlobalPosition.y-200)
					{
						currentCheckpoint = checkpoint;
						checkpoints.Remove(checkpoint);
						camera.queueLimit((int)(checkpoint.GlobalPosition.y));
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
						if(ball.GlobalPosition.y < flipper.GlobalPosition.y)
						{
							flipper.Unfold();
						}
					}
				}
			}
			if(ball.GlobalPosition.y > currentCheckpoint.GlobalPosition.y)
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
		if (Input.IsActionJustPressed("tap"))
		{
			if(starterHamster != null)
			{
				freeTheHamster();
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
				resume();
			} else {
				pause();
			}
			
		}
	}

	private void freeTheHamster()
	{
		starterHamster.TapTap();
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
		// var superSparkle = (SuperSparkle)sparkle.Instance();
		// if(balls.Count != 0)
		// {
		// 	for(int j = 0;j < balls.Count ; j++)
		// 	{
				
		// 		//var screenCord = (balls[j].GetViewportTransform() * balls[j].GetGlobalTransform()).Xform(balls[j].Position);
		// 		superSparkle.RectGlobalPosition = balls[j].GlobalPosition;
		// 	}
		// }
		// GetNode("HUDLayer").AddChild(superSparkle);
	}
	public int getPoints()
	{
		return points;
	}
	public void throwBall(Vector2 pos)
	{
		starterHamster = null;
		var scene = (PackedScene)ResourceLoader.Load("res://objects/ball/Ball.tscn");
		var newBall = (Ball)scene.Instance();
		balls.Add(newBall);
		newBall.GlobalPosition = pos;
		newBall.ApplyImpulse(GlobalPosition, new Vector2(0,-1) * 1024);
		AddChild(newBall);
		camera.dezoom();
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
	public HamsterStarter getStarter()
	{
		return starterHamster;
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
		GD.Print(mainLevel);
		points = 0;
		lives = 1;
		if(balls.Count != 0)
		{
			for(int j = 0;j < balls.Count ; j++)
			{
				var eraser = balls[j];
				eraser.GetParent().RemoveChild(this);
				eraser.QueueFree();
			}
		}
		balls = new List<Ball>();
		if(checkpoints.Count != 0)
		{
			for(int j = 0;j < checkpoints.Count ; j++)
			{
				var eraser = checkpoints[j];
				eraser.GetParent().RemoveChild(this);
				eraser.QueueFree();
			}
		}
		checkpoints = new List<Checkpoint>();
		if(flippers.Count != 0)
		{
			for(int j = 0;j < flippers.Count ; j++)
			{
				var eraser = flippers[j];
				eraser.GetParent().RemoveChild(this);
				eraser.QueueFree();
			}
		}
		flippers = new List<Flipper>();
		currentCheckpoint = null;
		removeAll(mainLevel);
		RemoveChild(mainLevel);
		mainLevel.QueueFree();
		var scene = (PackedScene)ResourceLoader.Load("res://levels/SuperLevel.tscn");
		var node = (Node2D)scene.Instance();
		camera.LimitBottom = 1000000;
		camera.queuedLimit = 1000000;
		AddChild(node);
		mainLevel = node;
		listerineChildren(mainLevel);
		if(checkpoints.Count != 0)
		{
			for(int j = 0;j < checkpoints.Count ; j++)
			{
				var checkpoint = checkpoints[j];
				if(currentCheckpoint == null)
					currentCheckpoint = checkpoint;
				if(checkpoint.GlobalPosition.y > currentCheckpoint.GlobalPosition.y)
				{
					currentCheckpoint = checkpoint;
				}
			}
		}
		GetTree().Paused = false;
		HUD.GetInstance().hideAll();
	}

	private void removeAll(Node node)
	{
		var childrenCount = node.GetChildCount();
		if(childrenCount == 0)
			return;
		
		for(int i = 0;i < node.GetChildCount() ; i++)
		{
			var child = node.GetChild(i);
			child.RemoveAndSkip();
			child.QueueFree();
		}
		removeAll(node);
	}
	private void listerineChildren(Node node)
	{
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
				if (child.GetType() == typeof(HamsterStarter))
				{
					starterHamster = (child as HamsterStarter);
				}
				if (child.GetType() == typeof(Level))
				{
					listerineChildren(child);
				}
			}
		}
	}
}
