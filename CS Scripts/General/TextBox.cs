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
	public static int dialogueNum = 0;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		dialogueNum = 0;
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
			dialogueNum++;
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
		else if (asking && Input.IsActionPressed("option_3") && timer.IsStopped()) {
			asking = false;
			Hide();
			EmitSignal(SignalName.ChoiceMade, "3");
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
		InactiveCountdown(dialogueNum);
		timer.Start();
		await ToSignal(this, TextBox.SignalName.ContinueDialogue);
	}
	
	//it's broken because idk how to get it to stop if player continues before timeout
	private async void InactiveCountdown(int curNum) {
		/*var timer2 = GetNode<Godot.Timer>("../../Timer2");
		timer2.Start(4.0f);*/
		await ToSignal(GetTree().CreateTimer(2f), SceneTreeTimer.SignalName.Timeout);
		if (curNum == dialogueNum) {
			EmitSignal(SignalName.PromptUser, this);
		}
	}
	
	public async Task<string> Ask(String text) {
		label.Clear();
		label.AppendText("[font_size=10]" + text + "[/font_size]");
		asking = true;
		Show();
		timer.Start();
		var result =  await ToSignal(this, TextBox.SignalName.ChoiceMade);
		/*GD.Print(result);///
		if (result is [Variant choice]) {
		 	return (string) choice;
		}*/
		return (string)result[0];
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
