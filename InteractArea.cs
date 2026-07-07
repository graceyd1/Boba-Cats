using Godot;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

public partial class InteractArea : Area2D
{
	[Export]
	public int labelHeight {get; set;} = 20;

	[Signal]
	public delegate void InteractEventHandler();

	[Signal]
	public delegate void EnterRoomEventHandler(String areaName);
	//if door - name used to decide which room to enter

	private Boolean playerNear;
	private Boolean allowInteraction;

	private Control interactLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		allowInteraction = true;

		//interactLabel =  GetTree().Root.GetNode<Node2D>("*").GetNodeOrNull<Control>("InteractLabel");	

		//looks up to 3 parent levels up to find interact label
		//there's probably a better way to do this
		Node level = (Node) this;
		for (int i = 0; i < 3; i ++)
		{
			interactLabel = level.GetParent().GetNodeOrNull<Control>("InteractLabel");
			if (interactLabel != null)
			{
				break;
			}
			try
			{
				level = (Node) level.GetParent();
			}
			catch
			{
				break;
			}
		}

		if (interactLabel != null)
		{
			interactLabel.Hide();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (playerNear)
		{
			if (Input.IsActionJustPressed("enter") && allowInteraction)
			{
				//interact signals
				EmitSignal(SignalName.Interact);
				EmitSignal(SignalName.EnterRoom, Name);
			}
		}
	}

	public void Interactable(Boolean allowInteract)
	{
		allowInteraction = allowInteract;
		if (interactLabel != null)
		{
			interactLabel.Hide();
		}
	}

	private void OnBodyEntered(Node2D body)
	{
		if (allowInteraction && interactLabel != null)
		{
			interactLabel.Position = new Vector2(GlobalPosition.X, GlobalPosition.Y - labelHeight);
			interactLabel.Show();
		}
		playerNear = true;
	}

	private void OnBodyExited(Node2D body)
	{
		if (interactLabel != null)
		{
			interactLabel.Hide();
		}
		playerNear = false;
	}	
}
