using Godot;
using System;
using System.Threading.Tasks;

public partial class JellyfishRoom : Node2D
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
			await NextRoomCheck();
		}
	}
	
	private async Task NextRoomCheck() {
		var FaderNode = GetNode<CanvasLayer>("/root/Fader");
		Vector2 pos = GetNode<CharacterBody2D>("UnderwaterPlayer").Position;
		var GlobalScript = GetNode<GlobalSceneChange>("/root/GlobalSceneChange");
		if (pos.X > 735) {
			transitioning = true;
			if (FaderNode is Fader fader) {
				await fader.FadeIn(.7f);
			}
			await GlobalScript.ChangeRoom(new Vector2(20, 135), "long_tube_coral_room", true);
		}
	}
}
