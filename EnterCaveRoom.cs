using Godot;
using System;
using System.Threading.Tasks;

public partial class EnterCaveRoom : Node2D
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
		var player = GetNode<CharacterBody2D>("GroundPlayer");
		var FaderNode = GetNode<CanvasLayer>("/root/Fader");
		var GlobalScript = GetNode<GlobalSceneChange>("/root/GlobalSceneChange");
		Vector2 pos = player.Position;
		if (pos.X > 315) {
			transitioning = true;
			if (FaderNode is Fader fader) {
				await fader.FadeIn(.7f);
			}
			await GlobalScript.ChangeRoom(new Vector2(10, 90), "cave_room", true);
		}
		// else if (pos.Y > 175) {
		// 	transitioning = true;
		// 	if (FaderNode is Fader fader) {
		// 		await fader.FadeIn(.7f);
		// 	}
		// 	await GlobalScript.ChangeRoom(new Vector2(idk, idk), "tall tube coral room", false);
		
		// }
	}
}
