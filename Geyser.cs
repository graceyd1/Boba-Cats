using Godot;
using System;

public partial class Geyser : Sprite2D
{
	private String CurrentAnimation;
	private AnimationPlayer Anim;
	private Area2D PullDownAOE;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Anim = GetNode<AnimationPlayer>("AnimationPlayer");
		CurrentAnimation = "up";
		Anim.Play(CurrentAnimation);
		PullDownAOE = GetNode<Area2D>("PullDownAOE");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	private void OnAnimationFinished()
	{
		if (CurrentAnimation == "up")
		{
			if (PullDownAOE.GetOverlappingBodies().Count > 0)
			{
				CurrentAnimation = "down_with_player";
			}
			else
			{
				CurrentAnimation = "down";
			}
		}
		else
		{
			CurrentAnimation = "up";
		}

		Anim.Play(CurrentAnimation);
	}
}
