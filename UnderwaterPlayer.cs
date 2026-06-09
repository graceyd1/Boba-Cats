using Godot;
using System;

public partial class UnderwaterPlayer : Player
{
	
	public override void _Ready() {
		base._Ready();
		base.Speed = 100;
		base.Gravity = 0.001F;
	}
	
	public void OnTubeCoralPull(Vector2 tubeVelocity)
	{
		setVelocityModifier(tubeVelocity);
	}

	//stop pulling the character when it leaves the AOE
	public void OnTubeCoralUnpull()
	{
		setVelocityModifier(Vector2.Zero);
	}

}
