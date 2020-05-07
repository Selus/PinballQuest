using Godot;
using System;

public class Cage : Node2D
{
    [Export] private int strength = 512;

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
        timer =  GetNode("Timer") as Timer;
        hamster =  GetNode("Hamster") as RigidBody2D;

        timer.Connect("timeout", this, "Timeout");
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


    public void Timeout()
    {

    }


    public void BodyEntered(Node node)
    {
        if (node.GetType() == typeof(Ball))
        {
			// bumper
			RigidBody2D rigid = node as RigidBody2D;
			Vector2 rigidDir = GlobalPosition.DirectionTo(rigid.GlobalPosition).Normalized();
			rigid.ApplyCentralImpulse(hamster.LinearVelocity.Normalized() * -strength);
			hamster.ApplyCentralImpulse(hamster.LinearVelocity.Normalized() * strength);
        }

        if (node.GetType() == typeof(Ball) && timer.TimeLeft == 0 && hit == 0)
		{
            timer.Start();

            // hits
            hit = 1;

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

            // points
            Main.GetInstance().addPoints(2);
        }
        
        if (node.GetType() == typeof(Ball) && timer.TimeLeft == 0 && hit == 1)
        {
            // points
            Main.GetInstance().addPoints(5);
            hamster.Sleeping = true;
            this.RemoveAndSkip();
        }
    }
}
