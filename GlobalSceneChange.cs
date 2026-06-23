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
		"BoxRoom"
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
		else if (n.Name == "UnderwaterPlayer" || n.Name == "GroundPlayer") {
			GD.Print("Player entered");
			EmitSignal(SignalName.SceneReady);
		}
	}

	
	public async void ChangeRoom(Vector2 pos, String room, bool right) {
		GetTree().ChangeSceneToFile("res://" + room + ".tscn");
		await ToSignal(this, GlobalSceneChange.SignalName.SceneReady);
		string roomName = GetTree().CurrentScene.Name;
		GD.Print(roomName);
		if (UnderwaterRooms.Contains(roomName)) {
			currPlayer = GetTree().CurrentScene.GetNode<Player>("UnderwaterPlayer");
		}
		else {
			currPlayer = GetTree().CurrentScene.GetNode<Player>("GroundPlayer");
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
