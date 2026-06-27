using Godot;
using System;
using System.Threading.Tasks;

public partial class Door : Area2D
{
	[Signal]
	public delegate void EnterRoomEventHandler(String roomName);

	private Boolean playerNear;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (playerNear)
		{
			if (Input.IsActionJustPressed("enter"))
			{
				GD.Print("Emmited signal");///
				EmitSignal(SignalName.EnterRoom, Name);
			}
		}
	}

	private void OnBodyEntered(Node2D body)
	{
		playerNear = true;
	}

	private void OnBodyExited(Node2D body)
	{
		playerNear = false;
	}	
}
