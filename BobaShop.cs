using Godot;
using System;

public partial class BobaShop : Node2D
{
	private TextBox dashT;
	private TextBox catssavaT;
	private AnimatedSprite2D csAnimation;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		dashT = GetNode<TextBox>("GroundPlayer/TextBox");
		catssavaT = GetNode<TextBox>("Catssava/TextBox");
		csAnimation = GetNode<AnimatedSprite2D>("Catssava");
		csAnimation.FlipH = true;
		StartDialogue();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public async void onExitRoom(string doorName) {
		var FaderNode = GetNode<CanvasLayer>("/root/Fader");
		var GlobalScene = GetNode<GlobalSceneChange>("/root/GlobalSceneChange");
		if (FaderNode is Fader fader) {
			await fader.FadeIn(.7f);
		}
		
		await GlobalScene.ChangeRoom(new Vector2(460, 170), "underwater_town", true);
	}
	
	public async void StartDialogue() {
		csAnimation.Animation = "sit";
		if (GlobalScript.metAzucat) {
			await catssavaT.ShowText("Oh hi there, I’m Catssava, the shopkeeper here. What can I help you with?");
			await dashT.ShowText("I need some tapioca boba, that’s all!");
			await catssavaT.ShowText("Oh dear, t-tapioca boba?!");
			await dashT.ShowText("That's right, Azucat told me it was fairly easy to get.");
			await catssavaT.ShowText("I-I-I'm really sorry, dear, but -");
			csAnimation.Animation = "cry";
			await catssavaT.ShowText("*[i]cries[/i]*");
			await dashT.ShowText("What's the matter? Is it something I said?");
			await catssavaT.ShowText("Well — this is embarrassing to say, since this is a boba shop…");
			await catssavaT.ShowText("But I'm all out of tapioca!");
			await dashT.ShowText("Is there...a way to get more?");
			await catssavaT.ShowText("I usually have to venture out into the open ocean to find ingredients, but they just keep disappearing!");
			await catssavaT.ShowText("Oh, if only I could find more tapioca...");
			await dashT.ShowText("Catssava, let me help you find the tapioca.");
			csAnimation.Animation = "sit";
			await catssavaT.ShowText("Really? You'd do that for me? I'd be soo grateful- what's you're name?");
			
			var choice = await dashT.Ask("1. Do it for Catssava\n2. Do it to get your boat fixed");

			if (choice == "1") {
				await dashT.ShowText("My name is Dash. And I'd be happy to help; I can tell you love this shop and it certainly needs some boba!");
			}
			else {
				await dashT.ShowText("My name is Dash. Azucat won't give me a new boat until I get him tapioca, so it's only right for me to do this.");
			}

		}
	}
}
