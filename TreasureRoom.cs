using Godot;
using System;
using System.Threading.Tasks;

public partial class TreasureRoom : Node2D
{
	private bool transitioning = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _Process(double delta)
	{
		if (!transitioning)
		{
			await NextRoomCheck();
		}
	}

	public async void GetBobaCutscene(Node2D player)
	{
		var azucat = GetNode<Sprite2D>("Azucat");
		var catssava = GetNode<Sprite2D>("Catssava");
		azucat.FlipH = true;
		var anim = GetNode<AnimationPlayer>("AnimationPlayer");

		var dashTextNode = player.GetNode<Node2D>("TextBox");
		if (dashTextNode is TextBox dText)
		{
			await dText.ShowText("Dialogue in progress :)");
		}

		anim.Play("cats_enter");
		await ToSignal(anim, AnimationPlayer.SignalName.AnimationFinished);
		azucat.FlipH = false;

		//animations: click on the AnimationPlayer in the editor and open the animation tab on the bottom

		//todo: dialogue with catssava and azucat
		

		//wowee you found the boba blah blah blah azucat has a new ship for dash
		//azucat picks up the entire boba!!!!!!!
		anim.Play("azucat_takes_the_boba");
		await ToSignal(anim, AnimationPlayer.SignalName.AnimationFinished);

		//more dialogue if we want

		//both cats leave
		anim.Play("cats_leave");
		await ToSignal(anim, AnimationPlayer.SignalName.AnimationFinished);
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
			await GlobalScript.ChangeRoom(new Vector2(620, 170), "sea_bunny_room", false);
		}

	}
}
