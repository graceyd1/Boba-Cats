using Godot;
using System;
using System.Threading.Tasks;

public partial class GeyserRoom : Node2D
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

	private async Task NextRoomCheck() {
		var player = GetNode<CharacterBody2D>("UnderwaterPlayer");
		var FaderNode = GetNode<CanvasLayer>("/root/Fader");
		var GlobalScript = GetNode<GlobalSceneChange>("/root/GlobalSceneChange");
		Vector2 pos = player.Position;
		if (pos.X < 5) {
			transitioning = true;
			if (FaderNode is Fader fader) {
				await fader.FadeIn(.7f);
			}

			if (pos.Y < 100)
			{
				await GlobalScript.ChangeRoom(new Vector2(300, 72), "tall_tube_coral_room", false);
			}
			else
			{
				await GlobalScript.ChangeRoom(new Vector2(620, 171), "fish_room", false);
			}
		}
	}
}
