using Godot;
using System;

public class paddle : KinematicBody2D
{
	[Export] private float angleEnd = -30;
	[Export] private float time = 3;

	private bool pressed = false;
	private float angleStart;
	Tween tween;


	public override void _Ready()
	{
		angleStart = RotationDegrees;
		tween = GetNode("Tween") as Tween;
	}


	public void Activate()
	{
		if (pressed)
		{
			pressed = false;
			Released();
		} else {
			pressed = true;
			Pressed();
		}
	}


	private void Pressed()
	{
		tween.Stop(this);
		float timeTo = time / Mathf.Abs(RotationDegrees - angleEnd);
		tween.InterpolateProperty(this, "rotation_degrees", RotationDegrees, angleEnd, timeTo);
		tween.Start();
	}


	private void Released()
	{
		tween.Stop(this);
		float timeTo = time / Mathf.Abs(RotationDegrees - angleStart);
		tween.InterpolateProperty(this, "rotation_degrees", RotationDegrees, angleStart, timeTo);
		tween.Start();
	}
}
