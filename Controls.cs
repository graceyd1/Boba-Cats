using Godot;
using System;

public partial class Controls : Node2D
{
	private ControlsTab cT;
	private QuestsTab qT;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var expanded = GetNode<Panel>("Expanded");
		expanded.Hide();
		cT = GetNode<ControlsTab>("ControlsTab");
		cT.Hide();
		GetNode<Button>("InventoryTab").Hide();
		qT = GetNode<QuestsTab>("QuestsTab");
		qT.Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		GlobalPosition = GetNode<Camera2D>("..").GetScreenCenterPosition() + new Vector2(120, -80);
	}
	
	public override void _Input(InputEvent @event) {
		if (Input.IsActionPressed("toggle_controls_menu")) {
			var expanded = GetNode<Panel>("Expanded");
			if (expanded.Visible) {
				expanded.Hide();
				cT.Hide();
				qT.Hide();
				GetNode<Button>("InventoryTab").Hide();
			}
			else {
				expanded.Show();
				cT.Show();
				qT.Show();
				GetNode<Button>("InventoryTab").Show();
				//default open controls tab
				cT.ButtonPressed = true;
				cT.OnButtonPressed();
			}
		}
	}
}
