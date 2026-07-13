using Godot;
using System;
using System.Threading.Tasks;

public partial class SeaBunnyRoom : Node2D
{
	private bool transitioning = false;
	private Player Player;
	private Seabunny SeaBunny;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Player =  GetNode<Player>("GroundPlayer");
		SeaBunny = GetNode<Seabunny>("Seabunny");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _Process(double delta)
	{
		//reset position when player leaves or respawns

		if (Player.respawning)
		{
			while (Player.respawning) {}
			SeaBunny.InFight = false;
			SeaBunny.Position = SeaBunny.StartPos;
		}

		if (!transitioning)
		{
			await NextRoomCheck();
		}
	}

	private void OnVineTimerTimeout()
	{
		var anim = GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("vine_appear");	
	}

	public void OnBossTriggerEntered(Node2D player)
	{
		GetNode<Godot.Timer>("VineTimer").Start();

		if (GlobalScript.CQ("short") == "ParvaCave")
		{
			GlobalScript.QuestNum ++;	
		}
		if (GlobalScript.CQ("short") == "Seabunny")
		{
			var camera = player.GetNode<Camera2D>("Camera2D");
			camera.PositionSmoothingEnabled = true;
			camera.PositionSmoothingSpeed = 5.0f;
			camera.GlobalPosition = new Vector2(320, camera.GlobalPosition.Y);
			while (camera.GlobalPosition.X < 320) {GD.Print("HI");}
			camera.SetLimit(Side.Left, 320);
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

				Player.SetDisableMovement(false);
				Player.invulnerable = false;
				player.Position = new Vector2(600, 145);
			}

			GlobalScript.QuestNum ++;
		}
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

	}
}
