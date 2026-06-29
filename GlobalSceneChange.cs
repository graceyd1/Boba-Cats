using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

public partial class GlobalSceneChange : Node2D
{
	public static Player currPlayer;
	
	[Signal]
	public delegate void SceneReadyEventHandler();
	
	public static List<string> UnderwaterRooms = new List<string> {
		"FirstRoom",
		"UnderwaterTown",
		"BoxRoom",
		"FishRoom",
		"LongTubeCoralRoom"
	};
	
	public static List<string> GroundRooms = new List<string> {
		"EnterCaveRoom",
		"CaveRoom",
		"SubmarineShop",
		"ParvaHouse",
		"EnterSeaBunnyRoom",
		"SeaBunnyRoom",
		"TreasureRoom",
		"BobaShop"
	};
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetTree().NodeAdded += checkNode;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void checkNode(Node n) {
		if (n == GetTree().CurrentScene) {
			GD.Print("root found");
			//EmitSignal(SignalName.SceneReady);
		}
		else if (n is Player) {
			EmitSignal(SignalName.SceneReady);
		}
	}

	
	public async Task ChangeRoom(Vector2 pos, String room, bool right) {
		GetTree().ChangeSceneToFile("res://" + room + ".tscn");
		await ToSignal(this, GlobalSceneChange.SignalName.SceneReady);
		string roomName = GetTree().CurrentScene.Name;
		if (UnderwaterRooms.Contains(roomName)) {
			currPlayer = GetTree().CurrentScene.GetNode<Player>("UnderwaterPlayer");
		}
		else if (GroundRooms.Contains(roomName)) {
			currPlayer = GetTree().CurrentScene.GetNode<Player>("GroundPlayer");
		}
		else {
			GD.Print("Room not found in list");
		}
		if (currPlayer == null) {
			GD.Print("ERROR getting player at GlobalSceneChange");
		}
		
		currPlayer.Position = pos;
		if (right) {
			currPlayer.facingRight = true;
		}
		else {
			currPlayer.facingRight = false;
		}
		var FaderNode = GetNode<CanvasLayer>("/root/Fader");
		if (FaderNode is Fader fade) {
			await fade.FadeOut(1.5f);
		}
	}
	
}
