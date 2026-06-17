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
		if (!invulnerable)
		{
			GetHit();
		}
	}

	//get hit
	private void GetHit()
	{
		hp --;
		if (hp <= 0)
		{
			GD.Print("You died!"); 
			Respawn();
			//Position = Vector2.Zero;
			hp = 2;
		}

		//i-frames
		var hurtTimer = GetNode<Godot.Timer>("HurtTimer");
		invulnerable = true;
		flash = true;
		hurtTimer.Start();

		EmitSignal(SignalName.Hit, hp);
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
				respawnPoint = new Vector2(156, 119);
			}
			else if (room == "TubesArea") {
				respawnPoint = new Vector2(45, 123);
			} 
			else if (room == "FirstRoom" || room == "BoxRoom") {
				respawnPoint = new Vector2(84, 100);
			}

			GlobalPosition = respawnPoint;
			
			await transition.FadeOut(2.0f);
		}
		
		
	}

	//when invulnerablility ends
	private void OnHurtTimerTimeout()
	{
		invulnerable = false;
		flash = false;

		var insideHurtbox =  GetNode<Area2D>("Hurtbox").GetOverlappingBodies();

		//if player is in hitbox when invulnerability ends
		if (insideHurtbox.Count > 0)
		{
			GetHit();
		}
	}

	public void setVelocityModifier(Vector2 vel)
	{
		velocityModifier = vel;
	}

	//moved to UnderwaterPlayer:
	// public void OnTubeCoralPull(Vector2 tubeVelocity)
	// {
	// 	velocityModifier = tubeVelocity;
	// }

	// //stop pulling the character when it leaves the AOE
	// public void OnTubeCoralUnpull()
	// {
	// 	velocityModifier = Vector2.Zero;
	// }
}
