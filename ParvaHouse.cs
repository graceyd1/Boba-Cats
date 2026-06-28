using Godot;
using System;
using System.Threading.Tasks;

public partial class ParvaHouse : Node2D
{
	private bool transitioning = false;
	private Node2D parvaTextN;
	private Node2D playerTextN;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		playerTextN = GetNode<Node2D>("GroundPlayer/TextBox");
		parvaTextN = GetNode<Node2D>("Parva/TextBox");
		startDialogue();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _Process(double delta)
	{
		if (!transitioning)
		{
			await NextRoomCheck();
		}
	}
	
	public async Task startDialogue() {
		if (playerTextN is TextBox plT && parvaTextN is TextBox paT) {
			await paT.showText("A [i]visitor[/i]. Well, I must say I'm surprised you got past the vines.");
			await paT.showText("You don't seem like one of those...[i]town cats[/i]. Why don't you come have a seat?");
			/*var player = GetNode<CharacterBody2D>("GroundPlayer");
			var pPos = player.Position;
			while (pPos.X != 121) {
				pPos = player.Position;
			}*/
			//lock player movement?
			await paT.showText("That's more like it. Now, what have you come all this way for?");
			await plT.showText("I'm looking for brown sugar boba. It seems like this place has every kind of boba except for that.");
			await plT.showText("I've looked everywhere.");
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
			await GlobalScript.ChangeRoom(new Vector2(467, 197), "cave_room", false);
		}
		else if (pos.Y > 175)
		{
			transitioning = true;
			if (FaderNode is Fader fader) {
				await fader.FadeIn(.7f);
			}
			await GlobalScript.ChangeRoom(new Vector2(138, 125), "enter_sea_bunny_room", true);
		}

	}
}
