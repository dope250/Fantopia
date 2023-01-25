using Godot;
using System;

public class player : KinematicBody2D
{
	public enum Looking
	{
		Left,
		Right
	}
	// Declare member variables here. Examples:
	[Export] public int playerID;
	[Export] public int health = 100;
	[Export] public int speed = 500;
	[Export] public PackedScene tombstone;
	[Export] public PackedScene bloodDecal;
	public Looking currentLooking;

	private bool disabledInput = false;
	private AnimatedSprite animatedSprite;
	private Camera2D camera;
	public Vector2 velocity = new Vector2();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		camera = GetNode<Camera2D>("Camera2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (!disabledInput)
		{
			GetInput();
		}
		velocity = MoveAndSlide(velocity);
	}

	public void ReceiveHit(int damage)
	{
		health = health - damage;
		if (health <= 0)
		{
			//Die();
		}
	}
	public void Die()
	{
		if (!(bloodDecal == null))
		{
			var bloodDecalInstance = (Node2D)bloodDecal.Instance();
			disabledInput = true;
			health = 0;
			Vector2 spawnPosition = this.Position;
			spawnPosition.y += 20;
			bloodDecalInstance.Position = spawnPosition;
			Owner.AddChild(bloodDecalInstance);
		}
		animatedSprite.Play("dying");
	}
	public void GetInput()
	{
		velocity = new Vector2();

		//Move/emote animation
		if (Input.IsActionPressed("p" + playerID + "_right"))
		{
			//animatedSprite.FlipH = false;
			velocity.x += 1;
			animatedSprite.Play("running_right");
			currentLooking = Looking.Right;
		}
		else if (Input.IsActionPressed("p" + playerID + "_left"))
		{
			velocity.x -= 1;
			//animatedSprite.FlipH = true;
			animatedSprite.Play("running_left");
			currentLooking = Looking.Left;
		}
		else if (Input.IsActionPressed("p" + playerID + "_up"))
		{
			velocity.y -= 1;
			animatedSprite.Play("running_up");
		}
		else if (Input.IsActionPressed("p" + playerID + "_down"))
		{
			velocity.y += 1;
			animatedSprite.Play("running_down");
		}
		else if (Input.IsActionPressed("p" + playerID + "_emote1"))
		{
			animatedSprite.Play("emote_yes");
		}
		else if (Input.IsActionPressed("p" + playerID + "_emote2"))
		{
			animatedSprite.Play("emote_no");
		}
		else if (Input.IsActionPressed("p" + playerID + "_emote3"))
		{
			animatedSprite.Play("emote_afraid");
		}
		else if (Input.IsActionPressed("p" + playerID + "_emote4"))
		{
			Die();
		}
		velocity = velocity.Normalized() * speed;


		//Set idle animation when move stopped
		if (Input.IsActionJustReleased("p" + playerID + "_right"))
		{
			animatedSprite.Play("idle_right");
		}
		else if (Input.IsActionJustReleased("p" + playerID + "_left"))
		{
			animatedSprite.Play("idle_left");
		}
		else if (Input.IsActionJustReleased("p" + playerID + "_up"))
		{
			animatedSprite.Play("idle_up");
		}
		else if (Input.IsActionJustReleased("p" + playerID + "_down"))
		{
			animatedSprite.Play("idle_down");
		}



	}
	private void _on_AnimatedSprite_animation_finished()
	{
		if (animatedSprite.Animation.Contains("dying") && disabledInput)
		{
			
			if (!(tombstone == null))
			{
				var tombstoneInstance = (Node2D)tombstone.Instance();
				tombstoneInstance.Position = this.Position;
				Owner.AddChild(tombstoneInstance);
			}

			//TODO: Signal player died; Fetch via gamecontroller script; set new camera on. If no other player is around, spawn new camera on last player position
			this.RemoveChild(camera);
			camera.Position = this.Position;
			Owner.AddChild(camera);
			QueueFree();
		}
		if (!animatedSprite.Animation.Contains("idle") && !disabledInput)
		{
			animatedSprite.Play("idle_down");
		}
	}

}



