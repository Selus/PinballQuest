using Godot;
using System;

public class Main : Node2D
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		generateMap();
	}
	
	private void generateMap()
	{
		var scene = (PackedScene)ResourceLoader.Load("res://levels/Level1.tscn");
		var node = (Node2D)scene.Instance();
		AddChild(node);
		Node2D lastNode;
		var scene2 = (PackedScene)ResourceLoader.Load("res://levels/Level2.tscn");
		var node2 = (Node2D)scene2.Instance();
		lastNode = node2;
		node2.Position = new Vector2(node.Position.x,node.Position.y - (node.GetNode("background1") as Sprite).GetRect().Size.y);
		AddChild(node2);
		for( int i = 0; i < 10 ; i++) 
		{
			scene2 = (PackedScene)ResourceLoader.Load("res://levels/Level2.tscn");
			node2 = (Node2D)scene2.Instance();
			node2.Position = new Vector2(lastNode.Position.x,lastNode.Position.y- (lastNode.GetNode("background1") as Sprite).GetRect().Size.y);
			AddChild(node2);
			lastNode = node2;
		}

		scene = (PackedScene)ResourceLoader.Load("res://objects/ball/Ball.tscn");
		node = (Node2D)scene.Instance();
		node.Position = new Vector2(200,-500);
		AddChild(node);
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
	}
}
