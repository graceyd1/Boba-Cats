using Godot;
using System;
using System.Threading.Tasks;

public partial class TextBox : Node2D
{
	[Signal]
	public delegate void ContinueDialogueEventHandler();
	
	[Signal]
	public delegate void ChoiceMadeEventHandler(String choice);
	
	[Signal]
	public delegate void PromptUserEventHandler(TextBox box);

	private Boolean showingText;
	private RichTextLabel label;
	private Godot.Timer timer;
	private Boolean asking;
	private bool inactive;
	
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
			inactive = false;
			Hide();
			EmitSignal(SignalName.ContinueDialogue);
		}
		else if (asking && Input.IsActionPressed("option_1") && timer.IsStopped()) {
			asking = false;
			Hide();
			EmitSignal(SignalName.ChoiceMade, "1");
		}
		else if (asking && Input.IsActionPressed("option_2") && timer.IsStopped()) {
			asking = false;
			Hide();
			EmitSignal(SignalName.ChoiceMade, "2");
		}
	}

	//show text
	public async Task ShowText(String text)
	{
		label.Clear();
		label.AppendText("[font_size=10]" + text + "[/font_size]");
	
		showingText = true;
		//GD.Print(Position); ///
		Show();
		
		inactive = true;
		InactiveCountdown();
		timer.Start();
		await ToSignal(this, TextBox.SignalName.ContinueDialogue);
	}
	
	//it's broken because idk how to get it to stop if player continues before timeout
	private async void InactiveCountdown() {
		/*var timer2 = GetNode<Godot.Timer>("../../Timer2");
		timer2.Start(4.0f);*/
		await ToSignal(GetTree().CreateTimer(0.5f), SceneTreeTimer.SignalName.Timeout);
		EmitSignal(SignalName.PromptUser, this);
	}
	
	public async Task<string> Ask(String text) {
		label.Clear();
		label.AppendText("[font_size=10]" + text + "[/font_size]");
		asking = true;
		Show();
		timer.Start();
		var result =  await ToSignal(this, TextBox.SignalName.ChoiceMade);
		if (result is [Variant choice]) {
			return (string) choice;
		}
		return null;
	}
	
	//so that pressing enter doesn't restart the conversation
	public void DisableInteractArea()
	{
		var parent = GetParent();
		if (parent is InteractArea area)
		{
			area.Interactable(false);
		}
	}

	public void EnableInteractArea()
	{
		var parent = GetParent();
		if (parent is InteractArea area)
		{
			area.Interactable(true);
		}
	}
}
