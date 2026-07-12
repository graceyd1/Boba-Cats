using Godot;
using System;
using System.Threading.Tasks;

public partial class SecretRoom1 : Node2D
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
		if (pos.Y > 175) {
			transitioning = true;
			if (FaderNode is Fader fader) {
				await fader.FadeIn(.7f);
			}
			await GlobalScript.ChangeRoom(new Vector2(395, 127), "vine_room", true);
		}
	}
}
