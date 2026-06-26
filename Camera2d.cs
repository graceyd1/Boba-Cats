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
		"ParvaHouse"
	};


	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (GetParent()?.GetParent().Name == "FirstRoom") {
			SetLimit(Side.Left, 0);
			SetLimit(Side.Right, 500);
			SetLimit(Side.Top, 0);
			SetLimit(Side.Bottom, 180);
		}

		///unused
		if (GetParent()?.GetParent().Name == "TestingRoom") {
			SetLimit(Side.Left, 0);
			SetLimit(Side.Right, 500);
			SetLimit(Side.Top, 0);
			SetLimit(Side.Bottom, 180);
		}

		if (FullScreen.Contains<String>(GetParent()?.GetParent().Name))
		{
			SetLimit(Side.Left, 0);
			SetLimit(Side.Right, 320);
			SetLimit(Side.Top, 0);
			SetLimit(Side.Bottom, 180);
		}
		
		if (GetParent()?.GetParent().Name == "BoxRoom")
		{
			SetLimit(Side.Left, 0);
			SetLimit(Side.Right, 500);
			SetLimit(Side.Top, 0);
			SetLimit(Side.Bottom, 180);
		}

		if (GetParent()?.GetParent().Name == "UnderwaterTown")
		{
			SetLimit(Side.Left, 0);
			SetLimit(Side.Right, 500);
			SetLimit(Side.Top, 0);
			SetLimit(Side.Bottom, 180);
		}

		if (GetParent()?.GetParent().Name == "FishRoom")
		{
			SetLimit(Side.Left, 0);
			SetLimit(Side.Right, 640);
			SetLimit(Side.Top, 0);
			SetLimit(Side.Bottom, 180);
		}

		if (GetParent()?.GetParent().Name == "CaveRoom")
		{
			SetLimit(Side.Left, 0);
			SetLimit(Side.Right, 500);
			SetLimit(Side.Top, 0);
			SetLimit(Side.Bottom, 300);
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
