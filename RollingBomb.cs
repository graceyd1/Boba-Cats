using Godot;
using System;

public partial class RollingBomb : RigidBody2D
{
	public float bombMass{get;} = 6.8f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Mass = bombMass; //in kilograms
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
