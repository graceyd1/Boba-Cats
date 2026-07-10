using Godot;
using System;

public partial class InventoryTab : Button
{
	private Label text;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		text = GetNode<Label>("../Expanded/MarginContainer/VBoxContainer/Label");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void OnButtonPressed() {
		text.Text = "";
		AddItem("flashlight", "Flashlight");
		AddItem("Town pass", "Town pass");
	}
	
	public void AddItem(string CodeName, string UIName) {
		if (GlobalScript.Inventory.Contains(CodeName)) {
			text.Text += "\n";
			text.Text += UIName;
		}
	}
}
