using Godot;
using System;

public class SuperSparkle : Control
{
	// Declare member variables here. Examples:
	// private int a = 2;
	// private string b = "text";

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var tween = (Tween)GetNode("Tween");
		tween.InterpolateProperty(this, "RectPosition",	this.RectPosition, new Vector2(1080/2,0), 1f,Tween.TransitionType.Quad,Tween.EaseType.In);
		tween.Start();
		tween.Connect("tween_completed", this, "TweenCompleted");
	}

	public void TweenCompleted(Node Node, NodePath key)
	{
		this.QueueFree();
	}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
