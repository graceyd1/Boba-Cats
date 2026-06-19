using Godot;
using System;

public partial class Seabunnybullet : Area2D
{
	public bool Left{get; set;}
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Left) {
			var pos = Position;
			pos.X -= 200 * (float)delta;
			Position = pos;
		}
		else {
			var pos = Position;
			pos.X += 200 * (float)delta;
			Position = pos;
		}
	}
}
