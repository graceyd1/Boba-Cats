//Most of this is the default CharacterBody2D code
using Godot;
using System;

public partial class GroundPlayer : Player
{
	public new float Speed = 150.0f;
	public const float JumpVelocity = -330.0f;
	public bool Climbing{get; set;} = false;

	private AnimatedSprite2D animatedSprite;
	public override void _Ready() {
		base._Ready();
		base.Speed = 150;
		base.Gravity = 0.002F;

		animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

	}

	public override void _PhysicsProcess(double delta)
	{
		if (!Climbing) {
			Vector2 velocity = Velocity;

			// Add the gravity.
			if (!IsOnFloor())
			{
				velocity += GetGravity() * (float)delta;
			}

			// Handle Jump.
			if (Input.IsActionJustPressed("move_up") && IsOnFloor())
			{
				velocity.Y = JumpVelocity;
			}

			// Get the input direction and handle the movement/deceleration.
			// As good practice, you should replace UI actions with custom gameplay actions.
			Vector2 direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
			if (direction != Vector2.Zero)
			{
				velocity.X = direction.X * Speed;
				if (direction.X < 0) {
					animatedSprite.Animation = "walk_left";
					facingRight = false;
				}
				else if (direction.X > 0) {
					animatedSprite.Animation = "walk_right";
					facingRight = true;
				}
				if (direction.Y != 0) {
					if (facingRight) {
						animatedSprite.Animation = "jump_right";
					}
					else {
						animatedSprite.Animation ="jump_left";
					}
				}
				animatedSprite.Play();
			}
			else
			{
				velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
				if (facingRight) {
					animatedSprite.Animation = "sit_right";
				}
				else {
					animatedSprite.Animation = "sit_left";
				}
				animatedSprite.Stop();
			}

			Velocity = velocity;
			MoveAndSlide();
		}
		else {
			var dir = Vector2.Zero;
			if (Input.IsActionPressed("move_up")) {
				dir.Y -= 1;
			}
			if (Input.IsActionPressed("move_down")) {
				dir.Y += 1;
			}
			if (Input.IsActionPressed("move_right")) {
				dir.X += 1;
			}
			if (Input.IsActionPressed("move_left")) {
				dir.X -= 1;
			}
			
			if (dir.Length() > 0) {
				dir = dir.Normalized() * Speed;
				
				animatedSprite.Play();
			}
			else {
				animatedSprite.Stop();
			}
			Velocity = dir;
			MoveAndSlide();
		}
		//flashing when hurt
		var hurtTimer = GetNode<Godot.Timer>("HurtTimer");
		if (flash)
		{
			var modulate = animatedSprite.Modulate;

			if (hurtTimer.TimeLeft % 0.2 < 0.1)
			{
				animatedSprite.Modulate = new Color(1, 1, 1, 1);
			}
			else
			{
				animatedSprite.Modulate = new Color(100, 1, 1, (float) 0.5);
			}
		}

		OOBCheck();
	}

	//change player back to normal after flashing animation
	public void OnHurtTimerTimeout()
	{
		animatedSprite.Modulate = new Color(1, 1, 1, 1);
	}

	//can change the code to go to different room instead
	private void OOBCheck() {
		var pos = GlobalPosition;
		if (GetParent().Name == "EnterCaveRoom" && pos.Y > 180) {
			base.Respawn(); //change this
		}
	}
}
