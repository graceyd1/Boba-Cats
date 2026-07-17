using Godot;
using System;
using System.Threading.Tasks;

public partial class PlantShop : Node2D
{
	private TextBox dText;
	private TextBox oText;
	private GroundPlayer classPlayer;
	public override void _Ready()
	{
		dText = GetNode<TextBox>("GroundPlayer/TextBox");
		oText = GetNode<TextBox>("Olive/TextBox");
		classPlayer = GetNode<GroundPlayer>("GroundPlayer");
		
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
		classPlayer.InputEnabled = false;
		classPlayer.GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation = "sit_left";
		//delete the placeholder stuff and write the dialogue
		//who are you? you're new here? welcome to my shop?
		await oText.ShowText("My oh my, a visitor. The last one tried to return their succulents after they withered.");
		await oText.ShowText("SUCCULENTS! No one ever appreciates the careful art of growing plants.");
		await oText.ShowText("They don't ever have the patience, and I doubt you'll be any different.");
		await oText.ShowText("So I won't be selling you any plants.");
		await oText.ShowText("Oh, I've forgotten to introduce myself. My name is 100% italian organic extra virgin olive oil.");
		await oText.ShowText("You may call me Olive.");
		await dText.ShowText("What can I buy then, if you won't sell me plants?");
		await oText.ShowText("I've got just the thing for you. A flashlight!");
		await dText.ShowText("How do I use it?");
		await oText.ShowText("You can control a flashlight with the mouse.");
		await oText.ShowText("It can also grow vines. Now I am going to demonstrate.");

		GetNode<Node2D>("Olive/Flashlight").Show();
		GetNode<CollisionShape2D>("Olive/Flashlight/Area2D/CollisionShape2D").Disabled = false;
		AnimatedSprite2D vineAnim = GetNode<AnimatedSprite2D>("GrowableVine/AnimatedSprite2D");
		await ToSignal(vineAnim, AnimatedSprite2D.SignalName.AnimationFinished);
		GetNode<Node2D>("Olive/Flashlight").Hide();

		await oText.ShowText("You'd better not be pot-headed enough to mess that up.");
		GlobalScript.OliveShopOpened = true;
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
				GlobalScript.Inventory.Add("Flashlight"); ///save?
				GlobalScript.Coins -= 10;
				await oText.ShowText("Here's your flashlight. I've given you the most basic one. It only has one mode.");
				await oText.ShowText("Press F to toggle the flashlight and use the mouse to change it's direction.");
				await oText.ShowText("You can view your inventory in the ESC menu.");
				await dText.ShowText("You really have that little trust in me?");
				await oText.ShowText("Gotten yourself stranded by hitting a [i]rock[/i] I've heard. I don't have high hopes.");
			}
			else
			{
				//eeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee
				///edit this one too?
				await oText.ShowText("You don't even have [i]ten[/i] measly coins? What even are you doing in my shop to begin with?");
				await oText.ShowText("Tut-tut, wasting my time I see. What an awful customer, just like the rest of them.");
			}
		}
		else if (choice == "2") {
			await oText.ShowText("Tut-tut, wasting my time I see. What an awful customer, just like the rest of them.");
		}
		else if (choice == "3") 
		{
			int success = GD.RandRange(0, 100); ///idk if I did this right
			if (success >= 99)
			{
				GlobalScript.Inventory.Add("Flashlight"); ///save?
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
				classPlayer.InputEnabled = true;
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
		classPlayer.InputEnabled = true;
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
