using Godot;
using System;
using System.Threading.Tasks;

public partial class UnderwaterTown : Node2D
{
	private bool transitioning = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _Process(double delta)
	{
		if (!transitioning) {
			await nextRoomCheck();
		}
	}
	
	private async Task nextRoomCheck() {
		var player = GetNode<CharacterBody2D>("UnderwaterPlayer");
		var FaderNode = GetNode<CanvasLayer>("/root/Fader");
		var GlobalScript = GetNode<GlobalSceneChange>("/root/GlobalSceneChange");
		Vector2 pos = player.Position;
		if (pos.X > 500) {
			transitioning = true;
			if (FaderNode is Fader fader) {
				await fader.FadeIn(1.5f);
			}
			await GlobalScript.ChangeRoom(new Vector2(10, 140), "box_room", true);
		}
		if (pos.X < 0) {
			transitioning = true;
			if (FaderNode is Fader fader) {
				await fader.FadeIn(1.5f);
			}
			await GlobalScript.ChangeRoom(new Vector2(490, 138), "first_room", false);
		
		}
	}
}
