using Godot;
using System;

public class StartButton : TextureButton
{


	public override void _Ready()
	{
		
	}

	public override void _Pressed()
	{
		Main.GetInstance().startGame();
	}


}
