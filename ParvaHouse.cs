using Godot;
using System;
using System.Threading.Tasks;

public partial class ParvaHouse : Node2D
{
	private bool transitioning = false;
	private Node2D parvaTextN;
	private Node2D dashTextN;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		dashTextN = GetNode<Node2D>("GroundPlayer/TextBox");
		parvaTextN = GetNode<Node2D>("Parva/TextBox");
		// startDialogue();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _Process(double delta)
	{
		if (!transitioning)
		{
			await NextRoomCheck();
		}
	}
	
	public async void startDialogue(Node2D player) {
		if (dashTextN is TextBox dT && parvaTextN is TextBox pT) {


			//lock player movement?
			//sure
			var dash = dashTextN.GetParent();
			if (dash is Player p)
			{
				p.setDisableMovement(true);
			}	
			player.Position = new Vector2(78, 132);

			await pT.showText("A [i]visitor[/i]. Well, I must say I'm surprised you got past the vines.");
			await pT.showText("You don't seem like one of those...[i]town cats[/i]. Why don't you come have a seat?");

			//move to seat:
			var animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
			if (dash is Player p1)
			{
				var sprite = GetNode<AnimatedSprite2D>("GroundPlayer/AnimatedSprite2D");
				sprite.Animation = "walk_right";
				sprite.Play();
			
				animationPlayer.Play("sit_down");
				await ToSignal(animationPlayer, AnimationPlayer.SignalName.AnimationFinished);

				sprite.Animation = "sit_right";
			}

			await pT.showText("That's more like it. My name is Parva. Now, what have you come all this way for, brown cat?");

			await dT.showText("I'm Dash. I've been on a quest to find the fabled brown sugar boba.");
			await dT.showText("Unfortunately, my ship crashed and I ended up underwater.");
			//await dT.showText("It seems like this place has every kind of boba except for that. I've looked everywhere.");
			//no they don't have every kind of boba because someone stole it all... lol

			//my idea:
			//parva says something along the lines of "I can help with that"
			await pT.showText("Hmm...I might be able to help you with that.");
			await pT.showText("You see, I have my own stash. It's top secret though; I trust you wouldn't tell anyone-");
			await pT.showText("Not that they would be able to get to it anyways, heh.");
			//trapdoor is revealed
			//opens the trapdoor:
			animationPlayer.Play("open_trapdoor");
			await ToSignal(animationPlayer, AnimationPlayer.SignalName.AnimationFinished);

			//player goes down
			//parva is in sea bunny room and tells player they got boba from the town
			//player gets mad
			//parva is like "in that case..." something
			//parva leaves and sea bunny fight starts

			if (dash is Player playerr)
			{
				playerr.setDisableMovement(false);
				playerr.inputEnabled = true;
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
