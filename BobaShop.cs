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
		StartDialogue();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public async void StartDialogue() {
		csAnimation.Animation = "sit";
		if (GlobalScript.metAzucat) {
			await catssavaT.showText("Oh hi there, I’m Catssava, the shopkeeper here. What can I help you with?");
			await dashT.showText("I need some tapioca boba, that’s all!");
			await catssavaT.showText("Oh dear, t-tapioca boba?!");
			await dashT.showText("That's right, Azucat told me it was fairly easy to get.");
			await catssavaT.showText("I-I-I'm really sorry, dear, but -");
			csAnimation.Animation = "cry";
			await catssavaT.showText("*[i]cries[/i]*");
			await dashT.showText("What's the matter? Is it something I said?");
			await catssavaT.showText("Well — this is embarrassing to say, since this is a boba shop…");
			await catssavaT.showText("But I'm all out of tapioca!");
			await dashT.showText("Is there...a way to get more?");
			await catssavaT.showText("I usually have to venture out into the open ocean to find ingredients, but they just keep disappearing!");
			await catssavaT.showText("Oh, if only I could find more tapioca...");
			await dashT.showText("Catssava, let me help you find the tapioca.");
			await catssavaT.showText("Really? You'd do that for me? I'd be soo grateful- what's you're name?");
			await dashT.ask("1. Do it for Catssava\n2. Do it to get your boat fixed");
			var ans = await ToSignal(dashT, TextBox.SignalName.ChoiceMade);
			if (ans is [Variant choice]) {
				if ((string)choice == "1") {
					await dashT.showText("My name is Dash. And I'd be happy to help; I can tell you love this shop and it certainly needs some boba!");
				}
				else {
					await dashT.showText("My name is Dash. Azucat won't give me a new boat until I get him tapioca, so it's only right for me to do this.");
				}
			}
		}
	}
}
