using Godot;
using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

public partial class InteractArea : Area2D
{
	[Export]
	public int LabelHeight {get; set;} = 20;
	
	[Export]
	public int DoorType {get; set;} //1 = enter, 0 = exit

	[Signal]
	public delegate void InteractEventHandler();

	[Signal]
	public delegate void InteractReturnAreaNameEventHandler(String areaName);
	//if door - name used to decide which room to enter

	private bool playerNear;
	
	private bool allowInteraction;

	private Control interactLabel;
	
	private List<string> ExitRooms = new List<string> {
		"BobaShop",
		"PlantShop",
		"SubmarineShop",
		"ParvaHouse"
	};

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		allowInteraction = true;
		//GD.Print(allowInteraction);
		
		//GD.Print(ExitRooms.Contains(GetNode<Node2D>("../..").Name));
		

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
		if (ExitRooms.Contains(Owner.Name)) {
			interactLabel.Scale = new Vector2(0.5f, 0.5f);
		}
		else {
			interactLabel.Scale = new Vector2(0.8f, 0.8f);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//GD.Print(allowInteraction);
		if (playerNear)
		{
			if (Input.IsActionJustPressed("enter") && allowInteraction)
			{
				GD.Print("interact");
				//interact signals
				EmitSignal(SignalName.Interact);
				EmitSignal(SignalName.InteractReturnAreaName, Name);
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
			interactLabel.Position = new Vector2(GlobalPosition.X, GlobalPosition.Y - LabelHeight);
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
