using Godot;
using System;
using System.Collections.Generic;

public partial class GlobalScript : Node2D
{
	//Eventually initialize to save file data
	public static int coins {get; set;} = 0;
	public static List<string> Inventory {get; set;} = new List<string>();
	
	//set because maybe we can change name of quest based on player choices
	public static List<string> MainQuests{get; set;} = new List<string> {
		"Find a mechanic to fix your ship",
		"Visit the boba shop and ask for brown sugar boba",
		"Journey into the dangerous ocean to find boba",
		"Escape from the sea bunny",
		"Return the boba to Azucat and Catssava"
	};
	
	public static int QuestNum{get; set;} = 0;


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
