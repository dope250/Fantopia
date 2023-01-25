using Godot;
using System;

public class npc : KinematicBody2D
{
	[Export] public int points = -5;
	[Export] public int health = 10;
	[Export] public int walkingSpeed = 100;
	[Export] public PackedScene bloodDecal;
	[Export] public PackedScene[] hitEffect = new PackedScene[0];

	private bool walkingFinished = true;
	private bool panicMode = false;
	private bool isDead = false;
	private AnimatedSprite animatedSprite;
	private gamecontroller gameController;
	public Vector2 target = new Vector2();
	public Vector2 velocity = new Vector2();

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		animatedSprite = GetNode<AnimatedSprite>("AnimatedSprite");
		animatedSprite.Playing = true;
		string[] mobTypes = animatedSprite.Frames.GetAnimationNames();
		animatedSprite.Animation = mobTypes[GD.Randi() % mobTypes.Length];
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		if (!isDead)
		{
			if (walkingFinished)
			{
				//GD.Print("Thinking about a new direction to go");
				target.x = GD.Randi() % 2000;
				target.y = GD.Randi() % 2000;
				GD.Print("X: " + target.x + " Y: " + target.y);
			}
			WalkAround();
		}
	}

	public void WalkAround()
	{
		int speed = walkingSpeed;
		if (panicMode)
		{
			speed = walkingSpeed * 3;
		} 
		else
		{
			speed = walkingSpeed;
		}
		
		
		velocity = new Vector2();
		velocity = Position.DirectionTo(target) * speed;

		//Set new target
		if (Position.DistanceTo(target) > 5)
		{
			walkingFinished = false;
			//GD.Print("Going for an walk");
			velocity = MoveAndSlide(velocity);
			Animation(velocity);

		}
		else
		{
			walkingFinished = true;
			//GD.Print ("Finished with my walking");
		}

	}

	public void Die()
	{
		isDead = true;
		if (!(bloodDecal == null))
		{
			var bloodDecalInstance = (Node2D)bloodDecal.Instance();
			health = 0;
			Vector2 spawnPosition = this.Position;
			spawnPosition.y += 20;
			bloodDecalInstance.Position = spawnPosition;
			Owner.AddChild(bloodDecalInstance);
		}
		animatedSprite.Play("dying");
		gameController = Owner.GetNode<gamecontroller>(".");
		gameController.Score += points;
	}

	private void Animation(Vector2 direction)
	{
		if (panicMode)
		{
			animatedSprite.SpeedScale = 1f;
		}
		else
		{
			animatedSprite.SpeedScale = 0.7f;
		}

		Vector2 normDirection = direction.Normalized();
		if (normDirection.y >= 0.707)
		{
			animatedSprite.Play("running_down");
		}
		else if (normDirection.y <= -0.707)
		{
			animatedSprite.Play("running_up");
		}
		else if (normDirection.x <= -0.707)
		{
			animatedSprite.Play("running_left");
		}
		else if (normDirection.x >= -0.707)
		{
			animatedSprite.Play("running_right");
		}
	}
	private void _on_AnimatedSprite_animation_finished()
	{
		if (animatedSprite.Animation.Contains("dying") && isDead)
		{
			QueueFree();
		}
		if (!animatedSprite.Animation.Contains("idle") && !isDead)
		{
			animatedSprite.Play("idle_down");
		}


	}

	public void ReceiveHit(int damage)
	{
		if (!(hitEffect == null))
		{
			var hitEffectInstance = (Node2D)hitEffect[GD.Randi() % hitEffect.Length].Instance();
			Vector2 spawnPosition = this.Position;
			hitEffectInstance.Position = spawnPosition;
			Owner.AddChild(hitEffectInstance);
		}

		GD.Print(this.Name + " received a +" + damage + " hit!");
		panicMode = true;
		health = health - damage;
		if (health <= 0)
		{
			Die();
		}
	}
}
