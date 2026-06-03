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
	
	//signal for turning the tubes off and on
	[Signal]
	public delegate void TubesSwitchEventHandler(float secs);
	
	private static bool tubesOn = true;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		EmitSignal(SignalName.TubesSwitch, 2.0f);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	//when a body (the player) enters the AOE, emit a signal
	//with the velocity the player should be pulled in
	private void OnAOEBodyEntered(CharacterBody2D body)
	{
		//do we need to check if the body is the player? idk
		//add later - multiple directions of corals
		if (tubesOn) {
			Vector2 velocity = new Vector2(0, (float).9);
			EmitSignal(SignalName.Pull, velocity);
		}
	}

	private void OnAOEBodyExited(CharacterBody2D body)
	{
		//do we need to check if the body is the player? idk
		if (tubesOn) {
			EmitSignal(SignalName.Unpull);
		}
	}
	
	private void OnTimerEnd() {
		//if on, switch to off, if off, switch to on
		tubesOn = !tubesOn;
		//should make timer start again with 2 seconds
		EmitSignal(SignalName.TubesSwitch, 2.0f);
	}
}
