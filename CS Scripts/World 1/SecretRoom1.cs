using Godot;
using System;
using System.Threading.Tasks;

public partial class SecretRoom1 : Node2D
{
	private bool transitioning = false;
	private AnimationPlayer anim;
	private TextBox gText;
	private int textIdx;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _Process(double delta)
	{
		anim = GetNode<AnimationPlayer>("AnimationPlayer");
		gText = GetNode<TextBox>("Green2/RightInteractArea/TextBox");
		textIdx = 0;

		if (!transitioning)
		{
			await NextRoomCheck();
		}
	}

	private void OnLeftInteract()
	{
		var area = GetNode<InteractArea>("Green/LeftInteractArea");
		area.Interactable(false);
		int random = GD.RandRange(0, 5);
		switch (random)
		{
			case 0: anim.Play("spin"); break;
			case 1: anim.Play("twirl"); break;
			case 2: anim.Play("jump"); break;
			case 3: anim.Play("squish"); break;
			case 4: anim.Play("fly"); break;
			case 5: anim.Play("skew"); break;
		}
		area.Interactable(true);
		if (area.playerNear)
		{
			GetNode<Control>("InteractLabel").Show();
		}
	}

	private async void OnRightInteract()
	{
		GD.Print("INTERACT");///

		gText.DisableInteractArea();
		switch (textIdx)
		{
			case 0: 
				await gText.ShowText("This is a secret room! Welcome!"); 
				GD.Print("DONE!");///
				if (GlobalScript.Azulcat)
				{
					await gText.ShowText("I heard a laser go off earlier. Was that you?");
				}
				break;
			case 1: 
				await gText.ShowText("These are the portraits of the creators of the game!"); 
				await gText.ShowText("Just kidding. This game was actually made by humans!");
				await gText.ShowText("But I'm sure these cats were a big help too, right?");
				break;
			case 2: 
				if (GlobalScript.QuestNum <= GlobalScript.MainQuests.IndexOf("ReturnBoba")) { //number
					await gText.ShowText("You're looking for a missing stash of tapioca pearls?");
					await gText.ShowText("Well, I haven't seen any around here. I hope you find it, though!");
				}
				else
				{
					await gText.ShowText("You found a stolen stash of tapioca pearls and returned it? Nice!");
				}
				break;
			case 3: 
				if (GlobalScript.QuestNum <= GlobalScript.MainQuests.IndexOf("ExploreCave")) { //number
					await gText.ShowText("Have you met Azucat? What an interesting cat!");
					await gText.ShowText("He used to be friends with another cat named Parva, but not anymore. I wonder what happened...");
				}
				else
				{
					await gText.ShowText("Have you noticed that Azucat's shop looks really similar to Parva's house?");
					await gText.ShowText("Maybe it's because Azucat and Parva used to be friends.");
					await gText.ShowText("Maybe it's because one of the creators of the game got lazy and copy-pasted.");
					await gText.ShowText("Or maybe it's a clue... to the [i]secret[/i] hiding in Azucat's shop...");

					if (GlobalScript.Azulcat)
					{
						await gText.ShowText("Maybe you've already found that secret.");
					}

					await gText.ShowText("Who knows?");
				}
			break;
		}
		textIdx ++;
		if (textIdx > 3)
		{
			textIdx = 0;
		}
		gText.EnableInteractArea();
	}

	private async Task NextRoomCheck() {
		var player = GetNode<CharacterBody2D>("UnderwaterPlayer");
		var FaderNode = GetNode<CanvasLayer>("/root/Fader");
		var GlobalScript = GetNode<GlobalSceneChange>("/root/GlobalSceneChange");
		Vector2 pos = player.Position;
		if (pos.Y > 175) {
			transitioning = true;
			if (FaderNode is Fader fader) {
				await fader.FadeIn(.7f);
			}
			await GlobalScript.ChangeRoom(new Vector2(395, 127), "vine_room", true);
		}
	}
}
