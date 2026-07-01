using Godot;
using System;
using System.Threading.Tasks;

public partial class SubmarineShop : Node2D
{
	private bool transitioning = false;
	Node2D playerTextNode;
	Node2D azucatTextNode;
	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		playerTextNode = GetNode<Node2D>("GroundPlayer/TextBox");
		azucatTextNode = GetNode<Node2D>("Azucat/TextBox");
		var player = GetNode<GroundPlayer>("GroundPlayer");
		var azucat = GetNode<Sprite2D>("Azucat");
		var sprite = GetNode<AnimatedSprite2D>("GroundPlayer/AnimatedSprite2D");

		if (player.Position.X > azucat.Position.X) {
			azucat.FlipH = true;
			sprite.Animation = "sit_left";
		}
		else {
			azucat.FlipH = false;
			sprite.Animation = "sit_right";
		}
		startDialogue(player);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _Process(double delta)
	{
		if (!transitioning) {
			
			await NextRoomCheck();
		}
	}
	
	public async void startDialogue(Node2D player) {
		if (playerTextNode is TextBox pText && azucatTextNode is TextBox aText) {
			if (!GlobalScript.MetAzucat) {
				if (player is Player p)
				{
					p.SetDisableMovement(true);
					/*var sprite = GetNode<AnimatedSprite2D>("GroundPlayer/AnimatedSprite2D");
					sprite.Animation = "sit_right";*/
				}

				await pText.ShowText("What is my ship doing on top of your roof??"); //fumi misty edit this stuff pls if you want
				await aText.ShowText("Oh sorry. I thought it looked cool, so I patched it up and put it there."); 
				await aText.ShowText("Didn't know it was yours."); 
				await pText.ShowText("How did you-- My ship just crashed! I need it to go back to the surface...");
				await aText.ShowText("How ‘bout let’s make a deal. You get some tapioca boba milk tea for me, and I’ll see what I can do ‘bout getting you a new boat.");
				await aText.ShowText("Don’t worry, it’s pretty easy to get. Best deal you’ll get ‘round here.");
				
				//Variant stores any data type (not used here anymore)
				var choice = await pText.Ask("Accept deal?\n1. Yes \n2. No");
				if (choice == "2") {
					await pText.ShowText("I don't know if I can trust you.");
					await aText.ShowText("I’m the only one who can make boats around here, so it’s not like you have a choice.");
					await pText.ShowText("...Fine.");
				}
				else {
					await pText.ShowText("Call it a deal!");
					await aText.ShowText("Pleasure doing business with you.");
				}

				//make player go through entire dialogue if they exited the shop in the middle of it
				GlobalScript.MetAzucat = true;

				if (player is Player p2)
				{
					p2.SetDisableMovement(false);
				}
			}
			else {
				await aText.ShowText("I trust that you're working on getting that boba for me?");
			}
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
			await GlobalScript.ChangeRoom(new Vector2(325, 525), "underwater_town", false);
		}
		if (pos.X > 315)
		{
			transitioning = true;
			if (FaderNode is Fader fader) {
				await fader.FadeIn(.7f);
			}
			await GlobalScript.ChangeRoom(new Vector2(402, 525), "underwater_town", true);
		}
	}
	
	public async void onExitRoom(string doorName) {
		var FaderNode = GetNode<CanvasLayer>("/root/Fader");
		var GlobalScene = GetNode<GlobalSceneChange>("/root/GlobalSceneChange");
		transitioning = true;
		if (FaderNode is Fader fader) {
			await fader.FadeIn(.7f);
		}
		
		if (doorName == "ShopExit")
		{
			await GlobalScene.ChangeRoom(new Vector2(326, 517), "underwater_town", true);
		}
	}
}
