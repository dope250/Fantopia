using Godot;
using System;

public class player_weapon : Area2D
{
	[Export]public int weaponDamage = 0;
	private AnimationPlayer weaponAnimation;
	private player currentPlayer;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		weaponAnimation = GetNode<AnimationPlayer>("Weapon_Animation");
		currentPlayer = GetParent<player>();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{
		GetWeaponInput();
	}
	private void GetWeaponInput()
	{
		//Attack animation
		if (Input.IsActionPressed("p" + currentPlayer.playerID + "_attack"))
		{
			if (currentPlayer.currentLooking == player.Looking.Right)
			{
				weaponAnimation.Play("attack_right");
			}
			else
			{
				weaponAnimation.Play("attack_left");
			}
		}
	}
	private void _on_Weapon_body_entered(KinematicBody2D body)
	{
		GD.Print("Hit: " + body.Name);

		if (body.Name.Contains("Player"))
		{
			//Do nothing; maybe hurt other players in the future to? Collision fixing needed beforehand
		}
		else if (body.Name.Contains("NPC"))
		{
			body.GetNode<npc>(".").ReceiveHit(weaponDamage);
		}
	}
}



