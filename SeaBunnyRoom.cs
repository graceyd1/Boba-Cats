using Godot;
using System;
using System.Threading.Tasks;

public partial class SeaBunnyRoom : Node2D
{
	private bool transitioning = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _Process(double delta)
	{
		if (!transitioning)
		{
			await NextRoomCheck();
		}
	}

	public async void EndFight(Node2D player)
	{
		player.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Stop();
		if (player is Player p)
		{
			p.SetDisableMovement(true);
			var seabunny = GetNode<CharacterBody2D>("Seabunny");
			if (seabunny is Seabunny sb)
			{
				await sb.EndFight();
			}

			p.SetDisableMovement(false);
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
