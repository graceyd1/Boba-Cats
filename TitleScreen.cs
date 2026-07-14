using Godot;
using System;

public partial class TitleScreen : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public async void OnPlayButtonPressed()
	{
		
		if (GlobalScript.QuestNum == 0) //todo - change condition? (it's probably fine)
		{
			//play opening animation cutscene
			GetNode<Control>("Buttons").Hide();
			var animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			animatedSprite.Animation = "opening";
			animatedSprite.Play();
			await ToSignal(animatedSprite, AnimatedSprite2D.SignalName.AnimationFinished);

			var FaderNode = GetNode<CanvasLayer>("/root/Fader");
			var GlobalScript = GetNode<GlobalSceneChange>("/root/GlobalSceneChange"); ///is this ok after I rearranged everything?
			if (FaderNode is Fader fader) {
				await fader.FadeIn(.7f);
			}
			await GlobalScript.ChangeRoom(new Vector2(35, 138), "first_room", true);
		}
		else
		{
			//todo - load the save data
		}

	}
}
