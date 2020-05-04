using Godot;
using System;

public class paddleTest : Node2D
{
    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_left"))
        {
            GetTree().CallGroup("paddleLeft", "Activate");
        }
        if (Input.IsActionJustReleased("ui_left"))
        {
            GetTree().CallGroup("paddleLeft", "Activate");
        }

        if (Input.IsActionJustPressed("ui_right"))
        {
            GetTree().CallGroup("paddleRight", "Activate");
        }
        if (Input.IsActionJustReleased("ui_right"))
        {
            GetTree().CallGroup("paddleRight", "Activate");
        }
    }
}
