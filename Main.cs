using Godot;
using System;

public class Main : Node2D
{
	private int points = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		instance = this;
		GetTree().Paused = true;
		generateMap();
	}

	private void generateMap()
	{
		var scene = (PackedScene)ResourceLoader.Load("res://levels/Level_0.tscn");
		var node = (Node2D)scene.Instance();
		node.PauseMode = Node.PauseModeEnum.Stop;
		AddChild(node);
		// Node2D lastNode;
		// var scene2 = (PackedScene)ResourceLoader.Load("res://levels/Level3.tscn");
		// var node2 = (Node2D)scene2.Instance();
		// lastNode = node2;
		// node2.Position = new Vector2(node.Position.x,node.Position.y - 2000);
		// node2.PauseMode = Node.PauseModeEnum.Stop;
		// AddChild(node2);
		// for( int i = 0; i < 10 ; i++) 
		// {
		// 	scene2 = (PackedScene)ResourceLoader.Load("res://levels/Level3.tscn");
		// 	node2 = (Node2D)scene2.Instance();
		// 	node2.Position = new Vector2(lastNode.Position.x,lastNode.Position.y-2000);
		// 	node2.PauseMode = Node.PauseModeEnum.Stop;
		// 	AddChild(node2);
		// 	lastNode = node2;
		// }

		// scene = (PackedScene)ResourceLoader.Load("res://objects/ball/Ball.tscn");
		// node = (Node2D)scene.Instance();
		// node.PauseMode = Node.PauseModeEnum.Stop;
		// node.Position = new Vector2(200,-500);
		// AddChild(node);
	}

	public override void _Process(float delta)
	{
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
