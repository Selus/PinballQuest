using Godot;
using System;

public class flipperTest : Node2D
{
	public override void _Process(float delta)
	{
		if (Input.IsActionJustPressed("ui_left"))
		{
			GetTree().CallGroup("flipperLeft", "Activate");
		}
		if (Input.IsActionJustReleased("ui_left"))
		{
			GetTree().CallGroup("flipperLeft", "Activate");
		}

		if (Input.IsActionJustPressed("ui_right"))
		{
			GetTree().CallGroup("flipperRight", "Activate");
		}
		if (Input.IsActionJustReleased("ui_right"))
		{
			GetTree().CallGroup("flipperRight", "Activate");
		}
	}
}
