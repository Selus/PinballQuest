using Godot;
using System;

public class Dandelion : Node2D
{

    RandomNumberGenerator random = new RandomNumberGenerator();
    AnimatedSprite animated;
    RigidBody2D rigid;

    public override void _Ready()
    {
		random.Seed = this.GetInstanceId() + (ulong)System.DateTime.Now.Second;
        animated = GetNode("RigidBody2D/AnimatedSprite") as AnimatedSprite;
        animated.Frame = random.RandiRange(0, 1);

        rigid = GetNode("RigidBody2D") as RigidBody2D;

        rigid.Connect("body_entered", this, "BodyEntered");
    }

    private void BodyEntered(Node node)
    {
        Main.GetInstance().addPoints(1);
    }
}
