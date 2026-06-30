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
			await NextRoomCheck();
		}
	}

	private async Task NextRoomCheck() {
		var player = GetNode<CharacterBody2D>("UnderwaterPlayer");
		var FaderNode = GetNode<CanvasLayer>("/root/Fader");
		var GlobalScript = GetNode<GlobalSceneChange>("/root/GlobalSceneChange");
		Vector2 pos = player.Position;
		if (pos.X > 495) {
			transitioning = true;
			if (FaderNode is Fader fader) {
				await fader.FadeIn(.7f);
			}
			await GlobalScript.ChangeRoom(new Vector2(10, 140), "box_room", true);
		}
		if (pos.X < 5) {
			transitioning = true;
			if (FaderNode is Fader fader) {
				await fader.FadeIn(.7f);
			}
			await GlobalScript.ChangeRoom(new Vector2(490, 520), "first_room", false);
		
		}
	}

	//shop doors:
	public async void OnEnterRoom(String roomName)
	{
		var player = GetNode<CharacterBody2D>("UnderwaterPlayer");
		var FaderNode = GetNode<CanvasLayer>("/root/Fader");
		var GlobalScene = GetNode<GlobalSceneChange>("/root/GlobalSceneChange");
		transitioning = true;
		if (FaderNode is Fader fader) {
			await fader.FadeIn(.7f);
		}
		
		if (roomName == "SubShopDoor")
		{
			await GlobalScene.ChangeRoom(new Vector2(66, 133), "submarine_shop", true);
		}
		if (roomName == "SubShopDoor2")
		{
			await GlobalScene.ChangeRoom(new Vector2(252, 133), "submarine_shop", false);
		}
		if (roomName == "BobaShopDoor") {
			await GlobalScene.ChangeRoom(new Vector2(65, 133), "boba_shop", true);
		}
	}
}
