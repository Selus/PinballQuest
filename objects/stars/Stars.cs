using Godot;
using System;

public class Stars : Area2D
{
	AnimationPlayer anim;

	public override void _Ready()
	{
		anim = GetNode("AnimationPlayer") as AnimationPlayer;
		anim.Play("scale");
	}
}
