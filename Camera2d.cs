using Godot;
using System;
using System.Linq;


public partial class Camera2d : Camera2D
{
	//all rooms that are 320x180 (full screen, no scrolling)
	private String[] FullScreen =
	{
		"SubmarineShop",
		"EnterCaveRoom",
		"ParvaHouse",
		"EnterSeaBunnyRoom",
		"TreasureRoom"
	};


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Node2D player = (Node2D) GetParent();

		if (player.GetParent().Name == "FirstRoom") {
			SetLimit(Side.Left, 0);
			SetLimit(Side.Right, 500);
			SetLimit(Side.Top, 0);
			SetLimit(Side.Bottom, 180);
		}

		///unused
		if (player.GetParent().Name == "TestingRoom") {
			SetLimit(Side.Left, 0);
			SetLimit(Side.Right, 500);
			SetLimit(Side.Top, 0);
			SetLimit(Side.Bottom, 180);
		}

		if (FullScreen.Contains<String>(player.GetParent().Name))
		{
			SetLimit(Side.Left, 0);
			SetLimit(Side.Right, 320);
			SetLimit(Side.Top, 0);
			SetLimit(Side.Bottom, 180);
		}
		
		if (player.GetParent().Name == "BoxRoom")
		{
			SetLimit(Side.Left, 0);
			SetLimit(Side.Right, 500);
			SetLimit(Side.Top, 0);
			SetLimit(Side.Bottom, 180);
		}

		else if (player.GetParent().Name == "UnderwaterTown")
		{
			SetLimit(Side.Left, 0);
			SetLimit(Side.Right, 500);
			SetLimit(Side.Top, 0);
			SetLimit(Side.Bottom, 550);
		}

		else if (player.GetParent().Name == "FishRoom")
		{
			SetLimit(Side.Left, 0);
			SetLimit(Side.Right, 640);
			SetLimit(Side.Top, 0);
			SetLimit(Side.Bottom, 180);
		}

		else if (player.GetParent().Name == "LongTubeCoralRoom")
		{
			SetLimit(Side.Left, 0);
			SetLimit(Side.Right, 400);
			SetLimit(Side.Top, 0);
			SetLimit(Side.Bottom, 180);
		}

		else if (player.GetParent().Name == "CaveRoom")
		{
			SetLimit(Side.Left, 0);
			SetLimit(Side.Right, 500);
			SetLimit(Side.Top, 0);
			SetLimit(Side.Bottom, 300);
		}

		else if (player.GetParent().Name == "SeaBunnyRoom")
		{
			SetLimit(Side.Right, 640);
			SetLimit(Side.Top, 0);
			SetLimit(Side.Bottom, 300);

			if (player.Position.X >= 320)
			{
				//camera stays still when in boss arena
				SetLimit(Side.Left, 320);
			}
			else
			{
				SetLimit(Side.Left, 0);
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
