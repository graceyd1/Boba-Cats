using Godot;
using System;
using System.Threading.Tasks;

public partial class PlantShop : Node2D
{
	private TextBox DashT;
	private TextBox OliveT;
	public override void _Ready()
	{
		DashT = GetNode<TextBox>("GroundPlayer/TextBox");
		OliveT = GetNode<TextBox>("Olive/TextBox");
		StartDialogue();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public async void StartDialogue() {
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
