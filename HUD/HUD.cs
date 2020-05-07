using Godot;
using System;

public class HUD : Control
{
	private static HUD instance=null;
	static public HUD GetInstance() 
	{
		return instance;
	}

	// Declare member variables here. Examples:
	private Label pointsText;
	private Label endPointsText;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		instance = this;
		pointsText = (Label)GetNode("Points");
		endPointsText = (Label)GetNode("GameOver/EndPoints");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		pointsText.Text = Main.GetInstance().getPoints().ToString();
		endPointsText.Text = Main.GetInstance().getPoints().ToString();
	}

	public void showMenu()
	{
		(GetNode("/root/Main/HUDLayer/HUD/Menu") as Control).Visible = true;
	}
	public void hideMenu()
	{
		(GetNode("/root/Main/HUDLayer/HUD/Menu") as Control).Visible = false;
	}
	public void showPauseMenu()
	{
		(GetNode("/root/Main/HUDLayer/HUD/Pause") as Control).Visible = true;
	}
	public void hidePauseMenu()
	{
		(GetNode("/root/Main/HUDLayer/HUD/Pause") as Control).Visible = false;
	}
	public void showGameOver()
	{
		(GetNode("/root/Main/HUDLayer/HUD/GameOver") as Control).Visible = true;
	}
	public void hideGameOver()
	{
		(GetNode("/root/Main/HUDLayer/HUD/GameOver") as Control).Visible = false;
	}
	public void hideAll()
	{
		(GetNode("/root/Main/HUDLayer/HUD/GameOver") as Control).Visible = false;
		(GetNode("/root/Main/HUDLayer/HUD/Pause") as Control).Visible = false;
		(GetNode("/root/Main/HUDLayer/HUD/Menu") as Control).Visible = false;
	}
}
