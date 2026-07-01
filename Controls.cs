using Godot;
using System;

public partial class Controls : Node2D
{
	private ControlsTab cT;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var expanded = GetNode<PanelContainer>("Expanded");
		expanded.Hide();
		cT = GetNode<ControlsTab>("ControlsTab");
		cT.Hide();
		GetNode<Button>("InventoryTab").Hide();
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
				cT.Hide();
				GetNode<Button>("InventoryTab").Hide();
			}
			else {
				expanded.Show();
				cT.Show();
				GetNode<Button>("InventoryTab").Show();
				cT.ButtonPressed = true;
				cT.OnButtonPressed();
			}
		}
	}
}
