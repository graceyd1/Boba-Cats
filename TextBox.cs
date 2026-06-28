using Godot;
using System;
using System.Threading.Tasks;

public partial class TextBox : Node2D
{
	[Signal]
	public delegate void ContinueDialogueEventHandler();
	
	[Signal]
	public delegate void ChoiceMadeEventHandler(String choice);

	private Boolean showingText;
	private RichTextLabel label;
	private Godot.Timer timer;
	private Boolean asking;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		showingText = false;
		label = GetNode<RichTextLabel>("PanelContainer/MarginContainer/RichTextLabel");
		timer = GetNode<Godot.Timer>("Timer");
		Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public override void _Input(InputEvent @event)
	{
		if (showingText && Input.IsActionPressed("enter") && timer.IsStopped())
		{
			showingText = false;
			Hide();
			EmitSignal(SignalName.ContinueDialogue);
		}
		else if (asking && Input.IsActionPressed("yes") && timer.IsStopped()) {
			asking = false;
			Hide();
			EmitSignal(SignalName.ChoiceMade, "yes");
		}
		else if (asking && Input.IsActionPressed("no") && timer.IsStopped()) {
			asking = false;
			Hide();
			EmitSignal(SignalName.ChoiceMade, "no");
		}
	}

	//show text
	public async Task showText(String text)
	{
		label.Clear();
		label.AppendText("[font_size=10]" + text + "[/font_size]");
	
		showingText = true;
		//GD.Print(Position); ///
		Show();
		
		timer.Start();
		await ToSignal(this, TextBox.SignalName.ContinueDialogue);
	}
	
	public async Task ask(String text) {
		label.Clear();
		label.AppendText("[font_size=10]" + text + "[/font_size]");
		asking = true;
		Show();
		timer.Start();
	}
	
}
