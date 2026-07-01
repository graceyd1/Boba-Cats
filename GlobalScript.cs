using Godot;
using System;
using System.Collections.Generic;

public partial class GlobalScript : Node2D
{
	//Eventually initialize to save file data
	public static int coins {get; set;} = 0;
	public static List<string> Inventory {get; set;} = new List<string>();
	public static bool MetAzucat{get; set;} = false;
	public static bool MetCatssava{get; set;} = false;

	public static bool FedToBunny {get; set;} = false;
	public static bool FinishedSeaBunny{get; set;} = false;

	public static bool GotSeaBoba{get; set;} = false;


	public static String savePath = "user://save_data.tres"; //not implemented yet (see GlobalSaveResource.cs)

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Inventory.Add("flashlight"); //testing purposes
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
