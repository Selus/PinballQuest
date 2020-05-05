using Godot;
using System;

public class Movable : Position2D
{
	Tween tween;

	[Export] Vector2 posOffset;
	[Export] float time = 4f;

	Tween.EaseType easy = Tween.EaseType.InOut;
	Tween.TransitionType trans = Tween.TransitionType.Cubic;

	Vector2 posStart;
	Vector2 posEnd;


	public override void _Ready()
	{
		tween = GetNode("Tween") as Tween;
		posStart = GlobalPosition;
		posEnd = GlobalPosition + posOffset;

		tween.InterpolateProperty(this, "position", posStart, posEnd, time, trans, easy);
		tween.InterpolateProperty(this, "position", posEnd, posStart, time, trans, easy, time);
		tween.Repeat = true;
		tween.Start();
	}
}
