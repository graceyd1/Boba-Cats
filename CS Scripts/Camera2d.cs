using Godot;
using System;
using System.Linq;
using System.Runtime.Intrinsics;


public partial class Camera2d : Camera2D
{
	//all rooms that are 320x180 (full screen, no scrolling)
	private String[] FullScreen =
	{
		"SubmarineShop",
		"BobaShop",
		"PlantShop",
		"SecretRoom1",
		"EnterCaveRoom",
		"ParvaHouse",
		"EnterSeaBunnyRoom",
		"TreasureRoom"
	};


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Node2D player = (Node2D) GetParent();

		SetLimit(Side.Left, 0);
		SetLimit(Side.Top, 0);
		
		//Zoom = new Vector2(1, 1);


		if (player.GetParent().Name == "FirstRoom") {
			SetLimit(Side.Right, 500);
			SetLimit(Side.Bottom, 180);
		}

		///unused
		if (player.GetParent().Name == "TestingRoom") {
			SetLimit(Side.Right, 500);
			SetLimit(Side.Bottom, 180);
		}

		if (FullScreen.Contains<String>(player.GetParent().Name))
		{
			SetLimit(Side.Right, 320);
			SetLimit(Side.Bottom, 180);
		}
		
		if (player.GetParent().Name == "BoxRoom")
		{
			SetLimit(Side.Right, 500);
			SetLimit(Side.Bottom, 180);
		}

		else if (player.GetParent().Name == "UnderwaterTown")
		{
			SetLimit(Side.Right, 500);
			SetLimit(Side.Bottom, 550);
		}

		else if (player.GetParent().Name == "FishRoom")
		{
			SetLimit(Side.Right, 640);
			SetLimit(Side.Bottom, 180);
		}

		else if (player.GetParent().Name == "LongTubeCoralRoom")
		{
			SetLimit(Side.Right, 400);
			SetLimit(Side.Bottom, 180);
		}

		else if (player.GetParent().Name == "JellyfishRoom")
		{
			SetLimit(Side.Right, 740);
			SetLimit(Side.Bottom, 220);
		}

		else if (player.GetParent().Name == "VineRoom")
		{
			SetLimit(Side.Right, 500);
			SetLimit(Side.Bottom, 180);
		}

		else if (player.GetParent().Name == "TallTubeCoralRoom")
		{
			SetLimit(Side.Right, 320);
			SetLimit(Side.Bottom, 600);
		}

		else if (player.GetParent().Name == "GeyserRoom")
		{
			SetLimit(Side.Right, 320);
			SetLimit(Side.Bottom, 900);
		}

		else if (player.GetParent().Name == "CaveRoom")
		{
			SetLimit(Side.Right, 500);
			SetLimit(Side.Bottom, 300);
		}
		
		if (player.GetParent().Name == "SeaBunnyRoom")
		{
			SetLimit(Side.Right, 640);
			SetLimit(Side.Bottom, 270);
			
			// if (player.Position.X >= 320)
			// {
			// 	//camera stays still when in boss arena
			// 	SetLimit(Side.Left, 320);
			// }
			if (true) /// todo to-do change to only happen when quest is before getting treasure
			{
				SetLimit(Side.Top, 90);
			}
		}
		else
		{
			PositionSmoothingEnabled = false;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Node2D player = (Node2D) GetParent();

		if (player.GetParent().Name == "SeaBunnyRoom")
		{
			if (player.Position.X < 310)
			{
				//camera moves like normal when not in boss arena
				SetLimit(Side.Left, 0);
			}
		}
	}
}
