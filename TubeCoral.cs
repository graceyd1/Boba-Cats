using Godot;
using System;

public partial class TubeCoral : Node2D
{
	//create a signal for when the player should get pulled
	//with the velocity of the pull
	[Signal]
	public delegate void PullEventHandler(Vector2 velocity);

	//signal for when the player should stop getting pulled
	[Signal]
	public delegate void UnpullEventHandler();
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	//when a body (the player) enters the AOE, emit a signal
	//with the velocity the player should be pulled in
	private void OnAOEBodyEntered(Node2D body)
	{
		//do we need to check if the body is the player? idk
		//add later - multiple directions of corals
		Vector2 velocity = new Vector2(0, (float).9);
		EmitSignal(SignalName.Pull, velocity);
	}

	private void OnAOEBodyExited(Node2D body)
	{
		//do we need to check if the body is the player? idk
		EmitSignal(SignalName.Unpull);
	}
}
