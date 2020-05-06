using Godot;
using System;
using System.Collections.Generic;

public class Main : Node2D
{
	private int points = 0;
	private BallCamera camera;
	private List<Ball> balls = new List<Ball>();
	private List<Level> checkpoints = new List<Level>();
	private Level currentCheckpoint;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		instance = this;
		camera = GetNode("BallCamera") as BallCamera;
		GetTree().Paused = true;
		generateMap();
	}

	private void generateMap()
	{
		var scene = (PackedScene)ResourceLoader.Load("res://levels/Level_0.tscn");
		var node = (Level)scene.Instance();
		node.PauseMode = Node.PauseModeEnum.Stop;
		AddChild(node);
		if(node.CheckPoint)
		{
			camera.LimitBottom = (int)(node.Position.y + node.Height);
			currentCheckpoint = node;
		}
		Level lastNode;
		var scene2 = (PackedScene)ResourceLoader.Load("res://levels/Level_1.tscn");
		var node2 = (Level)scene2.Instance();
		lastNode = node2;
		node2.Position = new Vector2(node.Position.x,node.Position.y-node2.Height);
		node2.PauseMode = Node.PauseModeEnum.Stop;
		AddChild(node2);
		scene2 = (PackedScene)ResourceLoader.Load("res://levels/Level_2.tscn");
		node2 = (Level)scene2.Instance();
		node2.Position = new Vector2(lastNode.Position.x,lastNode.Position.y-node2.Height);
		node2.PauseMode = Node.PauseModeEnum.Stop;
		AddChild(node2);
		if(node.CheckPoint)
		{
			checkpoints.Add(node2);
		}
		lastNode = node2;
		for( int i = 0; i < 10 ; i++) 
		{
			scene2 = (PackedScene)ResourceLoader.Load("res://levels/Level_1.tscn");
			node2 = (Level)scene2.Instance();
			node2.Position = new Vector2(lastNode.Position.x,lastNode.Position.y-node2.Height);
			node2.PauseMode = Node.PauseModeEnum.Stop;
			AddChild(node2);
			lastNode = node2;
		}
		scene = (PackedScene)ResourceLoader.Load("res://objects/ball/Ball.tscn");
		var nodeBall = (Node2D)scene.Instance();
		nodeBall.PauseMode = Node.PauseModeEnum.Stop;
		nodeBall.Position = new Vector2(300,-500);
		AddChild(nodeBall);
		balls.Add((Ball)nodeBall);
	}

	public override void _Process(float delta)
	{
		//ball tracker
		foreach(Ball ball in balls)
		{
			GD.Print(checkpoints.Count);
		 	if(checkpoints.Count != 0)
		 	{
		 		var checkpoint = checkpoints[0];
		 		if(ball.Position.y < checkpoint.Position.y)
		 		{
		 			currentCheckpoint = checkpoint;
		 			checkpoints.Remove(checkpoint);
		 			camera.LimitBottom = (int)(checkpoint.Position.y + checkpoint.Height);
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
