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
		"GoToTown", //0
		"MeetAzucat", //1
		"MeetCatssava", //2
		"ExploreOcean", //3
		"Trapdoor", //4
		"ParvaCave", //5 (after talking to parva) "Cave"
		"Seabunny", //6
		"ReturnBoba", //7
		"Surface" //8
	};
	
	public static int QuestNum{get; set;} = 1; //temp // todo: change to 0 later after fixing "that's my boat" cutscene
	
	public static Dictionary<string, string> QuestNames = new Dictionary<string, string> {
		{"GoToTown", "Explore the ocean"}, //0
		{"MeetAzucat", "Investigate the shop"}, //1
		{"MeetCatssava", "Visit the boba shop and ask for brown sugar boba"}, //2
		{"ExploreOcean", "Journey into the dangerous ocean to find boba"}, //3
		{"Trapdoor", "Meet Parva in his secret chambers"}, //4
		{"Cave", "Investigate the cave"}, //5 (after talking to parva)
		{"Seabunny", "Escape from the sea bunny"}, //6
		{"ReturnBoba", "Return the boba to Azucat and Catssava"}, //7
		{"Surface", "Head up to the surface"} //8
	};


	public static String savePath = "user://save_data.tres"; //not implemented yet (see GlobalSaveResource.cs)

	public static bool GeyserOpened {get; set;} = false;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	//get current quest
	//type = long or short
	public static String CQ(string type) {
		if (type == "long") {
			if (QuestNames.TryGetValue(MainQuests[QuestNum], out string fullName)) {
				return fullName;
			}
			else {
				GD.Print("Error getting current quest from QuestNames Dictionary");
				return null;
			}
		}
		else if (type == "short") {
			return MainQuests[QuestNum];
		}
		else {
			GD.Print("Parameter does not match short or long (GlobalScript.CQ(string))");
			return null;
		}
	}
}
