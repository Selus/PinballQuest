using Godot;
using System;

public class Flipper : KinematicBody2D
{
	[Export] private float angleEnd = -30;
	[Export] private float angleStart = 25;
	[Export] private float time = 3;
	[Export] public Boolean folded = true;

	private bool pressed = false;
	Tween tween;


	public override void _Ready()
	{
		if(folded == false)
		 	Unfold();
	}

	public void Unfold()
	{
		folded = false;
		RotationDegrees = angleStart;
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
		if(folded == false)
		{
			tween.Stop(this);
			float timeTo = time / Mathf.Abs(RotationDegrees - angleEnd);
			tween.InterpolateProperty(this, "rotation_degrees", RotationDegrees, angleEnd, timeTo);
			tween.Start();
		}
	}


	private void Released()
	{
		if(folded == false)
		{
			tween.Stop(this);
			float timeTo = time / Mathf.Abs(RotationDegrees - angleStart);
			tween.InterpolateProperty(this, "rotation_degrees", RotationDegrees, angleStart, timeTo);
			tween.Start();
		}
	}
}
