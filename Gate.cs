using Godot;
using System;

public partial class Gate : Node2D
{
	[Signal]
	public delegate void OpenEventHandler();

	private Sprite2D ButtonSprite;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		ButtonSprite = GetNode<Sprite2D>("Button/ButtonSprite");
	}

	// // Called every frame. 'delta' is the elapsed time since the previous frame.
	// public override void _Process(double delta)
	// {
	// }

	private void OnButtonPressed()
	{
		GetNode<AnimationPlayer>("AnimationPlayer").Play("open");
		EmitSignal(SignalName.Open);
	}

	private void OnButtonDown()
	{
		ButtonSprite.Frame = 1;
	}

	private void OnButtonUp()
	{
		ButtonSprite.Frame = 0;
	}
}
