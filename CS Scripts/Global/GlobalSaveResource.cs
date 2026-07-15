/* This class stores all of the player's save data
*  Saving and loading will be implemented in the GlobalScript
*/
using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

[Tool]
[GlobalClass]
public partial class GlobalSaveResource : Resource
{
	//configurations for a new game
	[Export]
	public int numCoins = 0;

	[Export]
	public Godot.Collections.Array<String> Inventory {get; set;} = new Array<String>(); 

	[Export]
	public int QuestNum{get; set;} = 0; //0;

	[Export]
	public bool GeyserOpened{get; set;} = false;
	
	[Export]
	public string CurrentRoom{get; set;} = "first_room";

	[Export]
	public int WorldNum{get; set;} = 1;
	
	[Export]
	public string DateSaved{get; set;}
	// [Export]
	// [Export]
	// [Export]
	// [Export]
	// [Export]
}
