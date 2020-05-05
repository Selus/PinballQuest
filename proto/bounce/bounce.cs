using Godot;
using System;

public class bounce : Position2D
{
    [Export] int iterations = 4;
    [Export] float strength = 1.1f;

    private Tween tween;


    public override void _Ready()
    {
        tween = GetNode("Tween") as Tween;
    }

     
    public void Emit()
    {
        
    }
}
