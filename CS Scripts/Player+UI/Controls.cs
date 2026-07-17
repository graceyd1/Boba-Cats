using Godot;
using System;

public partial class Controls : Node2D
{
	private ControlsTab cT;
	private QuestsTab qT;
	private Vector2 pos;
	private bool MenuOpen = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		pos = Position;
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
		GlobalPosition = GetNode<Camera2D>("..").GetScreenCenterPosition() + pos;
	}
	
	public override void _Input(InputEvent @event) {
		if (Input.IsActionJustPressed("toggle_controls_menu")) {
			var expanded = GetNode<Panel>("Expanded");
			if (MenuOpen) {
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
			MenuOpen = !MenuOpen;
		}
	}
}
