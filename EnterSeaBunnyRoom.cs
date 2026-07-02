using Godot;
using System;
using System.Threading.Tasks;

public partial class EnterSeaBunnyRoom : Node2D
{
	private bool transitioning = false;
	private TextBox dashT;
	private TextBox parvaT;
	//private static bool fedToBunny = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		dashT = GetNode<TextBox>("GroundPlayer/TextBox");
		parvaT = GetNode<TextBox>("Parva/TextBox");
		//Quest before escape from the sea bunny
		//note: alternatively could do GlobalScript.MainQuests[QuestNum] == quest name
		if (GlobalScript.QuestNum < 3) {
			GetNode<AnimatedSprite2D>("Parva").Show();
			StartDialogue();
		}
		else {
			GetNode<AnimatedSprite2D>("Parva").Hide();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _Process(double delta)
	{
		if (!transitioning)
		{
			await NextRoomCheck();
		}
	}
	
	private async Task StartDialogue() {
		var parvaAni = GetNode<AnimatedSprite2D>("Parva");
		parvaAni.Animation = "default";
		var player = GetNode<GroundPlayer>("GroundPlayer");
		//this way still affected by gravity
		player.InputEnabled = false;
		await dashT.ShowText("How'd you get an entire stash of boba?");
		await parvaT.ShowText("Ah, just stole them from the town's boba shop.");
		await dashT.ShowText("...");
		await parvaT.ShowText("No need to stare like that, it wasn't that hard. They keep their doors open at night, pfft.");
		
		var choice = await dashT.Ask("1. Stop Parva because stealing is wrong.\n2. Stop Parva so the town cats can get you a new boat");
		if (choice == "1") {
			await dashT.ShowText("I'm not staring because I'm [i]impressed[/i]! I'm staring because it's [i]wrong[/i] to steal like that!");
		}
		else {
			await dashT.ShowText("I may not be a 'town cat', but I've already made deals with them to fix my boat.");
			}

		await dashT.ShowText("Sorry, Parva, but I'm going to have to stop you.");
		parvaAni.Animation = "mad";
		await parvaT.ShowText("[i]HA![/i] Getting across the vines was one thing, but you [i]seriously[/i] think you can stop me?");

		if (choice == "1") {
			await parvaT.ShowText("Think again, goody-two-shoes! Let's see if your bite's up to your talk!");
		}
		else {
			await parvaT.ShowText("Think again, town-smoocher! You're going to regret this!");
		}

		var parva = GetNode<AnimatedSprite2D>("Parva");
		parva.Hide();
		await dashT.ShowText("...where did he go?");
		await dashT.ShowText("Seems like I'm stuck here. Maybe the boba here somewhere?");
		player.InputEnabled = true;
		//Next quest: defeat the sea bunny
		GlobalScript.QuestNum = 3;
	}

	private async Task NextRoomCheck() {
		var player = GetNode<CharacterBody2D>("GroundPlayer");
		var FaderNode = GetNode<CanvasLayer>("/root/Fader");
		var GlobalScript = GetNode<GlobalSceneChange>("/root/GlobalSceneChange");
		Vector2 pos = player.Position;
		if (pos.X < 150 && pos.Y < 70) {
			/*transitioning = true;
			if (FaderNode is Fader fader) {
				await fader.FadeIn(.7f);
			}
			await GlobalScript.ChangeRoom(new Vector2(201, 140), "parva_house", false);*/
			var dashT = GetNode<TextBox>("GroundPlayer/TextBox");
			await dashT.ShowText("It's too high up! I can't escape!");
		}
		else if (pos.X > 315)
		{
			transitioning = true;
			if (FaderNode is Fader fader) {
				await fader.FadeIn(.7f);
			}
			await GlobalScript.ChangeRoom(new Vector2(20, 231), "sea_bunny_room", true);
		}

	}
}
