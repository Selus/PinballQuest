using Godot;
using System;

public class Dandelion : Node2D
{

    RandomNumberGenerator random = new RandomNumberGenerator();
    AnimatedSprite animated;

    public override void _Ready()
    {
		random.Seed = this.GetInstanceId() + (ulong)System.DateTime.Now.Second;
        animated = GetNode("RigidBody2D/AnimatedSprite") as AnimatedSprite;
        animated.Frame = random.RandiRange(0, 1);

    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
