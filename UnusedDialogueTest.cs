using Godot;
using System;
using System.Threading.Tasks;

public partial class UnusedDialogueTest : Node2D
{
	Node2D dashTextNode;
	Node2D blueTextNode;
	AnimatedSprite2D blueSprite;

	// Called when the node enters the scene tree for the first time.
	public override async void _Ready()
	{
		dashTextNode = GetNode<Node2D>("UnderwaterPlayer/TextBox");
		blueSprite = GetNode<AnimatedSprite2D>("Blue");
		blueTextNode = GetNode<Node2D>("Blue/TextBox");

		await testDialogue();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public async Task testDialogue()
	{
		//blueText and dashText are the C# objects associated with their text box nodes
		if (blueTextNode is TextBox blueText && dashTextNode is TextBox dashText)
		{
			await blueText.showText("This is a dialogue test with words. I am saying words");
			await ToSignal(blueText, TextBox.SignalName.ContinueDialogue);

			await dashText.showText("Oh no! The font is blurry!");
			await ToSignal(dashText, TextBox.SignalName.ContinueDialogue);

			blueSprite.Animation = "red_eyes";
			await blueText.showText("There is too much going on in this room.");
		}
	}
}
