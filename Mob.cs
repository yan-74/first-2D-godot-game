using Godot;
using System;

public partial class Mob : RigidBody2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//randomly choose one of three animation types
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		string[] mobTypes = animatedSprite2D.SpriteFrames.GetAnimationNames();
		//pick random number between 0 and n-1
		animatedSprite2D.Play(mobTypes[GD.Randi() % mobTypes.Length]);
	}

	// Make mobs delete themselves when they leave the screen
	private void OnVisibleOnScreenNotifier2DScreenExited()
	{
		QueueFree();
	}
}
