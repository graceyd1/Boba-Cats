using Godot;
using System;
using System.Threading.Tasks;

public partial class PlantShop : Node2D
{
	private TextBox dText;
	private TextBox oText;
	public override void _Ready()
	{
		dText = GetNode<TextBox>("GroundPlayer/TextBox");
		oText = GetNode<TextBox>("Olive/TextBox");
		
		if (!GlobalScript.OliveShopOpened)
		{
			FirstShopDialogue();
		}
		else if (!GlobalScript.Inventory.Contains("flashlight"))
		{
			FlashlightShop();
		}
		else
		{
			///todo - something else?
		}

		GetNode<Hitbox>("Olive/Laser/Hitbox").SetDisabled(true);
		GetNode<CollisionShape2D>("Olive/Flashlight/Area2D/CollisionShape2D").Disabled = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	private async void FirstShopDialogue() {

		//delete the placeholder stuff and write the dialogue
		//who are you? you're new here? welcome to my shop?
		await oText.ShowText("A [i]visitor[/i]. Well, I must say I'm surprised you got past the vines.");
		await oText.ShowText("Why do I sound like Parva");
		await oText.ShowText("My name is 100% italian organic extra virgin olive oil.");
		await oText.ShowText("Call me Olive.");
		await dText.ShowText("How do you write dialogue");
		await oText.ShowText("I am advertising my flashlight on sale.");
		await oText.ShowText("You can control a flashlight with the mouse.");
		await oText.ShowText("It can also grow vines. Now I am going to demonstrate");

		GetNode<Node2D>("Olive/Flashlight").Show();
		GetNode<CollisionShape2D>("Olive/Flashlight/Area2D/CollisionShape2D").Disabled = false;
		AnimatedSprite2D vineAnim = GetNode<AnimatedSprite2D>("GrowableVine/AnimatedSprite2D");
		await ToSignal(vineAnim, AnimatedSprite2D.SignalName.AnimationFinished);
		GetNode<Node2D>("Olive/Flashlight").Hide();

		await oText.ShowText("You should buy it.");
		FlashlightShop();
	}

	private async void FlashlightShop()
	{
		await oText.ShowText("Do you want to buy the flashlight? It's 10 coins.");
		string choice = await dText.Ask("1. Buy the flashlight\n2. No thanks\n3. Steal the flashlight");
		if (choice == "1")
		{
			if (GlobalScript.Coins >= 10)
			{
				//eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee
				///edit this dialogue?
				GlobalScript.Inventory.Add("flashlight"); ///save?
				GlobalScript.Coins -= 10;
				await oText.ShowText("Here's your flashlight");
				await oText.ShowText("Press F to toggle the flashlight and use the mouse to change it's direction.");
				await oText.ShowText("You can view your inventory in the ESC menu.");
				await dText.ShowText("I got a flashlight!");
			}
			else
			{
				//eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee
				///edit this one too?
				await oText.ShowText("Not enough coins");
			}
		}
		else if (choice == "3") 
		{
			int success = GD.RandRange(0, 100); ///idk if I did this right
			if (success >= 99)
			{
				GlobalScript.Inventory.Add("flashlight"); ///save?
				await dText.ShowText("I stole a flashlight.");
			}
			else
			{
				//eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee
				await oText.ShowText("Hey");
				AnimatedSprite2D laser = GetNode<AnimatedSprite2D>("Olive/Laser");
				Hitbox laserHitbox = laser.GetNode<Hitbox>("Hitbox");
				laserHitbox.SetDisabled(false);
				laser.Show();
				laser.Animation = "start_laser";
				laser.Play();
				await ToSignal(laser, AnimatedSprite2D.SignalName.AnimationFinished);

				laser.Animation = "laser";
				laser.Play();

				var player = GetNode<Player>("GroundPlayer");
				await ToSignal(player, Player.SignalName.Died);
				var GlobalScene = GetNode<GlobalSceneChange>("/root/GlobalSceneChange");
				await GlobalScene.ChangeRoom(new Vector2(40, 321), "underwater_town", true);
				// laser.Hide();
				// laserHitbox.SetDisabled(true);
			}
		}
	}



	private async void OnExitRoom() {
		var FaderNode = GetNode<CanvasLayer>("/root/Fader");
		var GlobalScene = GetNode<GlobalSceneChange>("/root/GlobalSceneChange");
		if (FaderNode is Fader fader) {
			await fader.FadeIn(.7f);
		}
		
		await GlobalScene.ChangeRoom(new Vector2(40, 321), "underwater_town", true);
	}
}
