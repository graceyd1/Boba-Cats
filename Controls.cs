using Godot;
using System;

public partial class Controls : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (GlobalScript.Inventory.Contains("flashlight")) {
			var label = GetNode<Label>("Expanded/MarginContainer/Label");
			label.Text += "\n\nF - toggle flashlight";
		}
		var expanded = GetNode<PanelContainer>("Expanded");
		expanded.Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GlobalPosition = GetNode<Camera2D>("..").GetScreenCenterPosition() + new Vector2(120, -80);
	}
	
	public override void _Input(InputEvent @event) {
		if (Input.IsActionPressed("toggle_controls_menu")) {
			var expanded = GetNode<PanelContainer>("Expanded");
			if (expanded.Visible) {
				expanded.Hide();
			}
			else {
				expanded.Show();
			}
		}
	}
}
