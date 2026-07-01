using Godot;
using System;
using System.Data.SqlTypes;

public partial class Player : CharacterBody2D
{
	[Signal]
	public delegate void HitEventHandler(int hp);

	[Signal]
	public delegate void DiedEventHandler();

	public int Speed{get; set;}
	
	public float Gravity{get; set;}
	
	public float Mass = 4.54f; //in kg

	private int hp;

	//if true, player can't get hit
	public Boolean invulnerable;

	//used in cutscenes (implemented in the GroundPlayer and UnderwaterPlayer classes)
	private Boolean disableMovement;

	//player flashing animation (when hit)
	public Boolean Flash{get; set;}
	
	//determines player sitting position
	public Boolean FacingRight{get; set;}

	//stores velocity modifiers such as wind/tube coral pull
	public Vector2 VelocityModifier{get; set;}
	
	public bool InputEnabled{get;set;} = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//ScreenSize = GetViewportRect().Size;
		VelocityModifier = Vector2.Zero;
		hp = 2; //2;
		invulnerable = false;
		Flash = false;
		//gravity = Gravity.Underwater; //todo - change to update based on the player's room
	}
	
	//player enteres hitbox
	private void OnHurtboxAreaEntered(Node2D area)
	{
		if (GetNode<Godot.Timer>("HurtTimer").TimeLeft == 0) //bc OnTimerTimeout isn't working
		{
			invulnerable = false;
		}
		if (!invulnerable)
		{
			GetHit();
		}
	}

	//get hit
	private void GetHit()
	{
		hp --;
		if (hp <= 0 || GetParent().Name == "CaveRoom") // making you respawn after hit in cave room
		{
			// GD.Print("You died!"); 
			Respawn();
			EmitSignal(SignalName.Died);
			hp = 2;
		}

		//i-frames
		var hurtTimer = GetNode<Godot.Timer>("HurtTimer");
		invulnerable = true;
		Flash = true;
		hurtTimer.Start();

		EmitSignal(SignalName.Hit, hp);

	}
	
	//we need to make a list
	public async void Respawn() {
		var fader = GetNode<CanvasLayer>("/root/Fader");
		if (fader is Fader transition) {
			await ToSignal(GetTree().CreateTimer(1.0f), SceneTreeTimer.SignalName.Timeout);
			await transition.FadeIn(1.0f);
			
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
			else if (room == "BoxRoom")
			{
				respawnPoint = new Vector2(10, 140);
			}
			else if (room == "FishRoom")
			{
				respawnPoint = new Vector2(522, 122);
			}
			else if (room == "LongTubeCoralRoom")
			{
				respawnPoint = new Vector2(290, 135);
			}
			else if (room == "CaveRoom")
			{
				respawnPoint = new Vector2(25, 90);
			}
			else if (room == "SeaBunnyRoom")
			{
				respawnPoint = new Vector2(83, 232);
			}
			else
			{
				respawnPoint = new Vector2(20, 90);//default
			}

			GlobalPosition = respawnPoint;
			
			await transition.FadeOut(1.0f);
		}
		invulnerable = false;
		
	}

	//when invulnerablility ends
	//I don't know why but this method doesn't ever run for me

	private void OnHurtTimerTimeout()
	{
		invulnerable = false;
		Flash = false;

		var insideHurtbox =  GetNode<Area2D>("Hurtbox").GetOverlappingBodies();

		//if player is in hitbox when invulnerability ends
		if (insideHurtbox.Count > 0)
		{
			GetHit();
		}
	}

	public void SetVelocityModifier(Vector2 vel)
	{
		VelocityModifier = vel;
	}

	public void SetDisableMovement(Boolean disable)
	{
		disableMovement = disable;
		if (disable)
		{
			Velocity = Vector2.Zero;

		}
	}
	public bool MovementIsDisabled()
	{
		return disableMovement;
	}
	

}
