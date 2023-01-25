using Godot;
using System;

public class enemy : KinematicBody2D
{
	// Declare member variables here. Examples:
	[Export] public int health = 100;
	[Export] public int speed = 500;

	private AnimatedSprite animatedSprite;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
		{
			animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
		{
		
		}

	private void _on_AnimatedSprite_animation_finished()
	{
	if (animatedSprite.Animation != "idle")
		{
			animatedSprite.Play("idle");
		}
	}
}



