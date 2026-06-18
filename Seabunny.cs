using Godot;
using System;
using System.Threading.Tasks;

public partial class Seabunny : CharacterBody2D
{
	[Export]
	public int dashSpeed {get; set;} = 200;
	private AnimatedSprite2D animatedSprite;
	private Godot.Timer idleTimer;
	private Boolean facingLeft;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		facingLeft = true;
		Velocity = Vector2.Zero;
		
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite.Animation = "idle";
		animatedSprite.Play();

		idleTimer = GetNode<Godot.Timer>("IdleTimer");
		idleTimer.Start(); 
		
		GD.Print("Ready"); //
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();
	}

	//every time it is done waiting, do another attack
	private void OnIdleTimerTimeout()
	{
		GD.Print("Timer ended"); //
		doAttack();
	}

	private async Task doAttack()
	{
		int attack = GD.RandRange(0, 2);
		if (attack == 0)
		{
			await dash(1);
		}
		else if (attack == 1)
		{
			await dash(3);
		}
		else
		{
			await spin();
		}

		animatedSprite.Animation = "idle";
		idleTimer.Start();
	}

	//loops: how many times to loop the dashing animation
	private async Task dash(int loops)
	{
		GD.Print("small dash"); //
		animatedSprite.Animation = "start_dash";
		animatedSprite.Play(); //idk if we need to call Play() every time
		await ToSignal(animatedSprite, AnimatedSprite2D.SignalName.AnimationFinished);

		animatedSprite.Animation = "dashing";
		animatedSprite.Play();
		
		if (facingLeft)
		{
			Velocity = new Vector2(-dashSpeed, 0); //set velocity.x to -dashSpeed
		}
		else
		{
			Velocity = new Vector2(dashSpeed, 0);
		}

		//wait for dashing animation to loop a certain number of times
		for (int i = 0; i < loops; i ++)
		{
			GD.Print("Loop " + i); //
			await ToSignal(animatedSprite, AnimatedSprite2D.SignalName.AnimationLooped);
		}

		Velocity = Vector2.Zero;
		animatedSprite.Animation = "end_dash";
		animatedSprite.Play();
		await ToSignal(animatedSprite, AnimatedSprite2D.SignalName.AnimationFinished);

		//turn around
		if (facingLeft)
		{
			//turn to right
			facingLeft = false;
			animatedSprite.FlipH = true;
		}
		else
		{
			//turn to left
			facingLeft = true;
			animatedSprite.FlipH = false;
		}
	}

	private async Task spin()
	{
		GD.Print("spin"); //
	}
}
