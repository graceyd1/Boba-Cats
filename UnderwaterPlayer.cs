using Godot;
using System;

public partial class UnderwaterPlayer : Player
{
	
	public override void _Ready() {
		base._Ready();
		base.Speed = 100;
		base.Gravity = 0.001F;
	}
	
	public void OnTubeCoralPull(Vector2 velocity) {
		base.OnTubeCoralPull(velocity);
	}
	public void OnTubeCoralUnpull() {
		base.OnTubeCoralUnpull();
	}

}
