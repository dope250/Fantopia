using Godot;
using System;

public class SpawnPoint : Node2D
{
	[Export] PackedScene mobScene;
	[Export] int spawnQueue = 5;
	//NPC init
	[Export] public int points = -5;
	[Export] public int health = 10;
	[Export] public int walkingSpeed = 100;
	[Export] public PackedScene bloodDecal;
	[Export] public PackedScene[] hitEffect = new PackedScene[0];

	int alreadySpawned = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(float delta)
	{

	}
	void SpawnMob()
	{
		if (!(mobScene == null))
		{
			var mobSceneInstance = (npc)mobScene.Instance();
			Vector2 spawnPosition = this.Position;
			mobSceneInstance.Position = spawnPosition;
			mobSceneInstance.points = points;
			mobSceneInstance.health = health;
			mobSceneInstance.walkingSpeed = walkingSpeed;
			mobSceneInstance.bloodDecal = bloodDecal;
			mobSceneInstance.hitEffect = hitEffect;
			Owner.AddChild(mobSceneInstance);
		}
	}
	
	private void _on_Timer_timeout()
	{
		if (alreadySpawned < spawnQueue)
		{
			SpawnMob();
			alreadySpawned += 1;
		}
	}
}



