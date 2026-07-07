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
		"MeetAzucat", //0
		"MeetCatssava", //1
		"ExploreOcean", //2
		"Trapdoor", //3
		"Cave", //4 (after talking to parva)
		"Seabunny", //5
		"ReturnBoba", //6
		"Surface" //7
	};
	
	public static int QuestNum{get; set;} = 0;
	
	public static Dictionary<string, string> QuestNames = new Dictionary<string, string> {
		{"MeetAzucat", "Find a mechanic to fix your ship"}, //0
		{"MeetCatssava", "Visit the boba shop and ask for brown sugar boba"}, //1
		{"ExploreOcean", "Journey into the dangerous ocean to find boba"}, //2
		{"Trapdoor", "Meet Parva in his secret chambers"}, //3
		{"Cave", "Investigate the cave"}, //4 (after talking to parva)
		{"Seabunny", "Escape from the sea bunny"}, //5
		{"ReturnBoba", "Return the boba to Azucat and Catssava"}, //6
		{"Surface", "Head up to the surface"} //7
	};


	public static String savePath = "user://save_data.tres"; //not implemented yet (see GlobalSaveResource.cs)

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
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
