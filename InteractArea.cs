using Godot;
using System;
using System.Threading.Tasks;

public partial class InteractArea : Area2D
{
	[Signal]
	public delegate void InteractEventHandler();

	[Signal]
	public delegate void EnterRoomEventHandler(String areaName);
	//if door - name used to decide which room to enter

	private Boolean playerNear;

	private Boolean allowInteraction;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		allowInteraction = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (playerNear && allowInteraction)
		{
			if (Input.IsActionJustPressed("enter"))
			{
				//EmitSignal(SignalName.Interact);
				EmitSignal(SignalName.EnterRoom, Name);
				GD.Print(Name);
			}
		}
	}

	public void Interactable(Boolean allowInteract)
	{
		allowInteraction = allowInteract;
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
