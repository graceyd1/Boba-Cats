using Godot;
using System;
using System.Threading.Tasks;

public partial class Geyser : Node2D
{
	private AnimationPlayer Anim;
	// private Area2D PullDownAOE;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Anim = GetNode<AnimationPlayer>("AnimationPlayer");
		if (GetParent().GetNode<CharacterBody2D>("UnderwaterPlayer").Position.Y < 450)
		{
			Anim.Play("idle_top");
		}
		else{
			Anim.Play("RESET");
		}
		// PullAOE = GetNode<Area2D>("PullAOE");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnPullDownEntered(Node2D player)
	{
		if (Anim.CurrentAnimation == "idle_top" || !Anim.IsPlaying())
		{
			Anim.Play("down_with_player");
		}
	}

	public async void OnPullUpEntered(Node2D player)
	{
		if (!Anim.IsPlaying())
		{
			Anim.Play("up_with_player");
			await ToSignal(Anim, AnimationPlayer.SignalName.AnimationFinished);
			Anim.Play("idle_top");
		}
	}

// 	public async Task OnAnimationFinished(StringName animationName)
// 	{
// 		String currentAnimation = animationName;
// 		if (PullAOE.GetOverlappingBodies().Count > 0)
// 		{
// 			if (animationName == "up")
// 			{
// 				currentAnimation = "down_with_player";
// 			}
// 			else
// 			{
// 				currentAnimation = "up_with_player";
// 			}
// 		}
// 		else
// 		{
// 			if (animationName == "up")
// 			{
// 				currentAnimation = "down";
// 			}
// 			else
// 			{
// 				currentAnimation = "up";
// 			}
// 		}

// 		Anim.Play(currentAnimation);
// 	}
}
