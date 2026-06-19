using Godot;
using System;

public partial class Seabunnybullet : Area2D
{
	public Vector2 Velocity{get; set;}
	public float Speed = 200;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += Velocity.Normalized() * Speed * (float)delta;
	}
	
	public void HitPlayer(Node2D obj) {
		if (obj is Player player) {
			Hide();
		}
	}
}
