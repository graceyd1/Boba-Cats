using Godot;
using System;
using System.Threading.Tasks;

public partial class TextEnterLabel : Control
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public async void FadeIn(TextBox box) {
		Show();
		var label = GetNode<Label>("Label");
		label.Modulate = new Color(label.Modulate, 0.0f);
		var tween = GetTree().CreateTween();
		tween.TweenProperty(label, "modulate:a", 1.0, 0.5f);
		//await ToSignal(GetTree().CreateTimer(0.2f), SceneTreeTimer.SignalName.Timeout);
		await ToSignal(box, TextBox.SignalName.ContinueDialogue);
		Hide();
	}
}
