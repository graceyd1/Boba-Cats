using Godot;
using System;
using System.Threading.Tasks;

public partial class TextBox : Node2D
{
	[Signal]
	public delegate void ContinueDialogueEventHandler();

	private Boolean showingText;
	private RichTextLabel label;
	private Godot.Timer timer;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		showingText = false;
		label = GetNode<PanelContainer>("PanelContainer").GetChild<RichTextLabel>(0);
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
	}

	//show text
	public async Task showText(String text)
	{
		label.Clear();
		label.AppendText("[font_size=10]" + text + "[/font_size]");
		showingText = true;
		Show();
		timer.Start();
	}
	
}
