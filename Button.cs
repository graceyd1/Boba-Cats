using Godot;
using System;

public partial class Button : PanelContainer
{
	[Export]
	public string buttonName{get;set;}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var label = GetNode<Label>("Label");
		label.Text = buttonName;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
