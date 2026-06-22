using Godot;
using System;
using System.Threading.Tasks;

public partial class UnderwaterTown : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _Process(double delta)
	{
		await nextRoomCheck();
	}
	
	private async Task nextRoomCheck() {
		var FaderNode = GetNode<CanvasLayer>("/root/Fader");
		Vector2 pos = GetNode<CharacterBody2D>("UnderwaterPlayer").Position;
		if (pos.X > 500) {
			if (FaderNode is Fader fader) {
				await fader.FadeIn(1.5f);
			}
			GetTree().ChangeSceneToFile("res://box_room.tscn");
			if (FaderNode is Fader fade) {
				await fade.FadeOut(1.5f);
			}
		}
	}
}
