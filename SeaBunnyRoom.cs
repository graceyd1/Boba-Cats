using Godot;
using System;
using System.Threading.Tasks;

public partial class SeaBunnyRoom : Node2D
{
	private bool transitioning = false;
	private Node2D Player;
	private Node2D SeaBunny;


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Player =  GetNode<CharacterBody2D>("GroundPlayer");
		SeaBunny = GetNode<CharacterBody2D>("Seabunny");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _Process(double delta)
	{
		///todo to-do fix this
		// if (Player.Position.X <= 300)
		// {
		// 	if (SeaBunny is Seabunny sb)
		// 	{
		// 		sb.InFight = false;
		// 	}
		// }

		if (!transitioning)
		{
			await NextRoomCheck();
		}

		///GD.Print(GetNode<Godot.Timer>("VineTimer").TimeLeft);///
	}

	private void OnVineTimerTimeout()
	{
		var anim = GetNode<AnimationPlayer>("AnimationPlayer");
		anim.Play("vine_appear");	
	}

	public void StartFight(Node2D Player)
	{
		GetNode<Godot.Timer>("VineTimer").Start();
		if (SeaBunny is Seabunny sb)
		{
			sb.StartFight();
		}
	}
	public async void EndFight(Node2D player)
	{
		player.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Stop();
		
		if (player is GroundPlayer p)
		{
			p.invulnerable = true;
			p.SetDisableMovement(true);
			if (SeaBunny is Seabunny sb)
			{
				GD.Print("Ending");///
				sb.InFight = false;
				await sb.EndFight();
			}

			p.SetDisableMovement(false);
			p.invulnerable = false;
			player.Position = new Vector2(600, 145);
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
