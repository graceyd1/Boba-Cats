using Godot;
using System;
using System.Threading.Tasks;

public partial class Seabunny : CharacterBody2D
{
	[Export]
	public int dashSpeed {get; set;} = 200;

	// [Export]
	// public String animationOverride {get; set;} = null;

	public bool InFight;

	private AnimatedSprite2D animatedSprite;
	private Godot.Timer idleTimer;
	private Boolean facingLeft;
	private PackedScene bullet;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		facingLeft = true;
		Velocity = Vector2.Zero;
		bullet = GD.Load<PackedScene>("res://seabunnybullet.tscn");
		GD.Randomize();
		
		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		animatedSprite.Animation = "idle";
		animatedSprite.Play();

		idleTimer = GetNode<Godot.Timer>("IdleTimer");
		InFight = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
		MoveAndSlide();

		//prevent it from leaving the arena
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, 333, 600),
			y: Position.Y
		);

		// //animation override for cutscenes
		// if (animationOverride != null)
		// {
		// 	GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation = animationOverride;
		// }
	}

	public async void StartFight()
	{
		Position = new Vector2(485, 213);

		if (!InFight)
		{
			InFight = true;
			idleTimer.Start(); 
			await ToSignal(idleTimer, Godot.Timer.SignalName.Timeout);
		}
		else
		{
			InFight = false;
		}
	}

	public async Task EndFight()
	{
		InFight = false;

		if (GetNode<Area2D>("Hitbox") is Hitbox h)
		{
			h.SetDisabled(true);
		}
		
		var anim = GetParent().GetNode<AnimationPlayer>("AnimationPlayer");

		Velocity = Vector2.Zero;
		
		while (Position.X < 540)
		{
			facingLeft = false;
			GD.Print(facingLeft);///
			await Dash(1);
		}
		Position = new Vector2(550, 198);

		animatedSprite.Animation = "start_climb";
		animatedSprite.Play();
		await ToSignal(animatedSprite, AnimatedSprite2D.SignalName.AnimationFinished);

		animatedSprite.Animation = "climbing";
		animatedSprite.Play();
		anim.Play("climb_vine");
		await ToSignal(anim, AnimationPlayer.SignalName.AnimationFinished);

		animatedSprite.Animation = "end_climb";
		animatedSprite.Play();
		anim.Play("fall");
		await ToSignal(anim, AnimationPlayer.SignalName.AnimationFinished);

		animatedSprite.Animation = "sleep";
		animatedSprite.Play();
	}

	//every time it is done waiting, do another attack
	private async void OnIdleTimerTimeout()
	{
		if (InFight)
		{
			await DoAttack();
		}
	}

	private async Task DoAttack()
	{
		int attack = GD.RandRange(0, 3);
		if (attack == 0)
		{
			await Dash(1);
		}
		else if (attack == 1)
		{
			await Dash(3);
		}
		else
		{
			await Spin();
		}

		animatedSprite.Animation = "idle";
		idleTimer.Start();
	}

	//loops: how many times to loop the dashing animation
	private async Task Dash(int loops)
	{
		animatedSprite.Animation = "start_dash";
		animatedSprite.Play(); //idk if we need to call Play() every time
		await ToSignal(animatedSprite, AnimatedSprite2D.SignalName.AnimationFinished);

		animatedSprite.Animation = "dashing";
		animatedSprite.Play();
		
		GD.Print(facingLeft);///
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

	private async Task Spin()
	{
		animatedSprite.Animation = "spin";
		animatedSprite.Play();
		await ToSignal(GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);
		
		//clone bullets, positioned left to right
		AddBullet(new Vector2(0, Rand(12, 32)), new Vector2(-1, 0));
		AddBullet(new Vector2(0, Rand(-8, 8)), new Vector2(-1, -1));
		AddBullet(new Vector2(Rand(0, 16), 0), new Vector2(0, -1));
		AddBullet(new Vector2(Rand(51, 60), Rand(0, 10)), new Vector2(1, -1));
		AddBullet(new Vector2(63, Rand(20, 33)), new Vector2(1, 0));
		
		await ToSignal(animatedSprite, AnimatedSprite2D.SignalName.AnimationFinished);
	}
	
	private static int Rand(int low, int high) {
		var randomizer = new RandomNumberGenerator();
		randomizer.Randomize();
		return randomizer.RandiRange(low, high);
	}
	
	//position: relative to origin of sea bunny (parent)
	private void AddBullet(Vector2 position, Vector2 velocity) {
		Seabunnybullet inst = bullet.Instantiate<Seabunnybullet>();
		inst.Position = position;
		inst.Velocity = velocity;
		AddChild(inst);
	}
}
