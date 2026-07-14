using Godot;
using System;
using System.Threading.Tasks;

public partial class SeaBunnyRoom : Node2D
{
	private bool transitioning = false;
	private Player Player;
	private Seabunny SeaBunny;
	private bool cameraGliding = false;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Player =  GetNode<Player>("GroundPlayer");
		SeaBunny = GetNode<Seabunny>("Seabunny");

		if (GlobalScript.CQ("short") == "GetBoat")
		{
			GetBoatCutscene();	
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _Process(double delta)
	{
		//reset position when player respawns
		if (Player.respawning && SeaBunny.InFight)
		{
			//somehow wait for player to finish respawning before resetting
			//while (Player.respawning) {} //breaks the game

			SeaBunny.InFight = false;
			SeaBunny.Position = SeaBunny.StartPos;
		}

		if (!transitioning)
		{
			await NextRoomCheck();
		}
		
		if (cameraGliding) {
			if (IsInstanceValid(Player)) //trying to fix the "Cannot access a disposed object" exception
			{
				var camera = Player.GetNode<Camera2D>("Camera2D");
				if (camera.GetTargetPosition() == camera.GetScreenCenterPosition()) {
					camera.PositionSmoothingEnabled = false;
					cameraGliding = false;
				}
			}
		}
	}

	private void OnVineTimerTimeout()
	{
		var anim = GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("vine_appear");	
	}

	public void OnBossTriggerEntered(Node2D player)
	{
		if (GlobalScript.CQ("short") == "ParvaCave")
		{
			GlobalScript.QuestNum ++;	
		}
		if (GlobalScript.CQ("short") == "Seabunny")
		{
			GetNode<Godot.Timer>("VineTimer").Start();
			var camera = player.GetNode<Camera2D>("Camera2D");
			camera.PositionSmoothingEnabled = true;
			camera.PositionSmoothingSpeed = 5.0f;
			cameraGliding = true;
			camera.GlobalPosition = new Vector2(470, camera.GlobalPosition.Y);
			
			//camera.SetLimit(Side.Left, 320);
			
			SeaBunny.StartFight();

		}
	}
	public async void EndFight(Node2D player)
	{
		if (GlobalScript.CQ("short") == "Seabunny")
		{
			player.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Stop();

			if (!Player.respawning)
			{
				Player.invulnerable = true;
				Player.SetDisableMovement(true);

				SeaBunny.InFight = false;
				await SeaBunny.EndFight();

				if (Player is GroundPlayer gp)
				{
					gp.Climbing = true;
				}
				Player.SetDisableMovement(false);
				Player.invulnerable = false;

				GlobalScript.QuestNum ++;
			}
		}
	}

	private async void GetBoatCutscene()
	{
		var azucatBoba = GetNode<Sprite2D>("BOBA");
		azucatBoba.Position = new Vector2(218, 75);
		var aText = GetNode<TextBox>("BOBA/Azucat/TextBox");
		var anim = GetNode<AnimationPlayer>("AnimationPlayer");
		Player.Position = new Vector2(573, 232);
		Player.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation = "sit_left";
		Player.SetDisableMovement(true);
		await aText.ShowText("Thanks for your help, Dash! Here's your new ship!");
		
		anim.Play("ship_deployed");
		await ToSignal(anim, AnimationPlayer.SignalName.AnimationFinished);

		await GetNode<TextBox>("GroundPlayer/TextBox").ShowText("...");
		await aText.ShowText("...Well, I better get going! See ya!");

		anim.Play("azucat_leaves");
		await ToSignal(anim, AnimationPlayer.SignalName.AnimationFinished);
		Player.SetDisableMovement(false);
	}

	private async Task NextRoomCheck() {
		var player = GetNode<CharacterBody2D>("GroundPlayer");
		var FaderNode = GetNode<CanvasLayer>("/root/Fader");
		var GlobalScript = GetNode<GlobalSceneChange>("/root/GlobalSceneChange");
		Vector2 pos = player.Position;
		if (pos.X < 5) {
			transitioning = true;
			if (FaderNode is Fader fader) {
				await fader.FadeIn(.7f);
			}
			await GlobalScript.ChangeRoom(new Vector2(160, 125), "enter_sea_bunny_room", false);
		}
		else if (pos.X > 635)
		{
			transitioning = true;
			if (FaderNode is Fader fader) {
				await fader.FadeIn(.7f);
			}
			await GlobalScript.ChangeRoom(new Vector2(20, 140), "treasure_room", true);
		}
		else if (pos.Y < 5)
		{
			transitioning = true;
			if (FaderNode is Fader fader) {
				await fader.FadeIn(.7f);
			}
			await GlobalScript.ChangeRoom(Vector2.Zero, "world_end_screen", true);
		}

	}
}
