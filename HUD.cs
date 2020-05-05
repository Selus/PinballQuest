using Godot;
using System;

public class HUD : Control
{
	// Declare member variables here. Examples:
	private Label pointsText;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		pointsText = (Label)GetNode("Points");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		pointsText.Text = Main.GetInstance().getPoints().ToString();
	}
}
