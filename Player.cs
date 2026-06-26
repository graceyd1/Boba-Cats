using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Signal]
	public delegate void HitEventHandler(int hp);

	public int Speed{get; set;}
	
	public static int coins {get; set; } = 0;
	
	public float Gravity{get; set;}
	
	public float Mass = 4.54f; //in kg



	//not sure if this should be private
	public int hp;

	//if true, player can't get hit
	private Boolean invulnerable;

	//player flashing animation (when hit)
	public Boolean flash{get; set;}
	
	//determines player sitting position
	public Boolean facingRight{get; set;}

	//stores velocity modifiers such as wind/tube coral pull
	public Vector2 velocityModifier{get; set;}

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//ScreenSize = GetViewportRect().Size;
		velocityModifier = Vector2.Zero;
		hp = 2;
		invulnerable = false;
		flash = false;
		//gravity = Gravity.Underwater; //todo - change to update based on the player's room
	}

	
	//player enteres hitbox
	private void OnHurtboxAreaEntered(Node2D area)
	{
		GD.Print("hitbox entered");
		if (!invulnerable)
		{
			GetHit();
		}
		else
		{
			GD.Print("invulnerable");
			GD.Print(GetNode<Godot.Timer>("HurtTimer").TimeLeft);
		}
	}

	//get hit
	private void GetHit()
	{
		hp --;
		if (hp <= 0 || GetParent().Name == "CaveRoom") // making you respawn after hit in cave room)
		{
			// GD.Print("You died!"); 
			Respawn();
			hp = 2;
		}

		//i-frames
		var hurtTimer = GetNode<Godot.Timer>("HurtTimer");
		invulnerable = true;
		flash = true;
		hurtTimer.Start();

		EmitSignal(SignalName.Hit, hp);
		GD.Print("Timer started");
		GD.Print(GetNode<Godot.Timer>("HurtTimer").TimeLeft);

	}
	
	public async void Respawn() {
		var fader = GetNode<CanvasLayer>("/root/Fader");
		if (fader is Fader transition) {
			await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
			await transition.FadeIn(2.0f);
			
			Vector2 respawnPoint = Vector2.Zero;
			var room = GetParent().Name;
			if (room == "EnterCaveRoom") {
				//coorinates might change if room coordinates change
				respawnPoint = new Vector2(113, 122);
			}
			else if (room == "TubesArea") {
				respawnPoint = new Vector2(45, 123);
			} 
			else if (room == "FirstRoom" || room == "BoxRoom") {
				respawnPoint = new Vector2(10, 140);
			}
			else if (room == "SeabunnyBossRoom") {
				respawnPoint = new Vector2(100, 144);
			}
			else if (room == "UnderwaterTown") {
				respawnPoint = new Vector2(10, 518);
			}
			else if (room == "CaveRoom")
			{
				respawnPoint = new Vector2(10, 90);
			}

			GlobalPosition = respawnPoint;
			
			await transition.FadeOut(1.5f);
		}
		invulnerable = false;
		
	}

	//when invulnerablility ends
	private void OnHurtTimerTimeout()
	{
		GD.Print("Times up"); ///
		invulnerable = false;
		flash = false;

		var insideHurtbox =  GetNode<Area2D>("Hurtbox").GetOverlappingBodies();

		//if player is in hitbox when invulnerability ends
		if (insideHurtbox.Count > 0)
		{
			GetHit();
		}
		else
		{
			GD.Print("safe now");///
		}
	}

	public void setVelocityModifier(Vector2 vel)
	{
		velocityModifier = vel;
	}

}
