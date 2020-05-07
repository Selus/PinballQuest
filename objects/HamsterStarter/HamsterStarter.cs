using Godot;
using System;

public class HamsterStarter : Node2D
{
	[Export] private int strength = 300;

	private int hit = 0;

	AnimationPlayer animEyes;
	AnimationPlayer animHead;

	Node2D hookA;
	Node2D hookB;
	Polygon2D ropeDraw;
	Vector2[] ropePolygons = new Vector2[4];
	[Export] Vector2 ropeOffset;

	Timer timer;
	RigidBody2D hamster;


	public override void _Ready()
	{
		hamster =  GetNode("Hamster") as RigidBody2D;

		hamster.Connect("body_entered", this, "BodyEntered");

		animEyes = GetNode("AnimEyes") as AnimationPlayer;
		animHead = GetNode("AnimHead") as AnimationPlayer;

		hookA = GetNode("Hook") as Node2D;
		hookB = GetNode("Hamster/Hook") as Node2D;
		ropeDraw = GetNode("RopeDraw") as Polygon2D;

		animEyes.Play("Eyes");
		animHead.Play("Head");

		ropeDraw.LookAt(hamster.GlobalPosition);
	}


	public override void _Process(float delta)
	{
		ropeDraw.LookAt(hamster.GlobalPosition);
	}



	public void TapTap()
	{
		var random = new RandomNumberGenerator();
		random.Seed = this.GetInstanceId() + (ulong)System.DateTime.Now.Second;
		var tx = random.RandiRange(0, 1);
		if(tx == 0) tx = -1;
		hamster.ApplyCentralImpulse(new Vector2(tx, 0) * strength);

		if (hit < 3)
		{
			// hits
			hit++;

			animEyes.PlaybackSpeed = 0.25f;
			animHead.PlaybackSpeed = 0.25f;

			AnimatedSprite EyeL = GetNode("Hamster/Position2D/Hamster_head/EyeL") as AnimatedSprite;
			AnimatedSprite EyeR = GetNode("Hamster/Position2D/Hamster_head/EyeR") as AnimatedSprite;
			AnimatedSprite PupilL = GetNode("Hamster/Position2D/Hamster_head/EyeR/Pupil") as AnimatedSprite;
			AnimatedSprite PupilR = GetNode("Hamster/Position2D/Hamster_head/EyeR/Pupil") as AnimatedSprite;
			AnimatedSprite CageFront = GetNode("Hamster/Position2D/CageFront") as AnimatedSprite;

			EyeL.Frame = 1;
			EyeR.Frame = 1;
			PupilL.Frame = 1;
			PupilR.Frame = 1;
			CageFront.Frame = 1;
		}
		
		if (hit == 3)
		{
			GD.Print("Start");
			Main.GetInstance().throwBall(hamster.GlobalPosition);
			// points
			Main.GetInstance().addPoints(5);
			hamster.Sleeping = true;
			this.GetParent().RemoveChild(this);
			this.QueueFree();
			
		}
	}
}
