using Godot;
using System;

public partial class GrowableVine : Area2D
{
	[Export]
	public bool GrownByDefault {get; set;} = false;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if (!GrownByDefault)
		{
			animatedSprite2D.Animation = "ungrown";
			GetNode<CollisionShape2D>("GrownCollisionShape").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void Eaten()
	{
		GetNode<CollisionShape2D>("GrownCollisionShape").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		GetNode<PointLight2D>("PointLight2D").Hide();
		GetNode<AnimatedSprite2D>("AnimatedSprite2D").Animation = "eaten";
	}

	public async void OnAreaEntered(Area2D flashlight) {
		var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		if (animatedSprite2D.Animation == "ungrown") {
			animatedSprite2D.Animation = "growing";
			animatedSprite2D.Play();

			GetNode<AnimationPlayer>("AnimationPlayer").Play("grow");

			//the collision shape now enables before it fully grows (in case you want to speedrun)
			GetNode<CollisionShape2D>("CollisionShape2D").SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
			GetNode<CollisionShape2D>("GrownCollisionShape").SetDeferred(CollisionShape2D.PropertyName.Disabled, false);

			await ToSignal(animatedSprite2D, AnimatedSprite2D.SignalName.AnimationFinished);
			animatedSprite2D.Animation = "grown";
		}
	}
	public void OnBodyEntered(Node2D player) {
		if (player is GroundPlayer groundPlayer) {
			var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
			if (animatedSprite2D.Animation == "grown") {
				groundPlayer.Climbing = true;
			}
		}
	}
	public void OnBodyExited(Node2D player) {
		if (player is GroundPlayer groundPlayer) {
			groundPlayer.Climbing = false;
		}
	}
}
