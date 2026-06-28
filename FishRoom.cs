using Godot;
using System;
using System.Threading.Tasks;

public partial class FishRoom : Node2D
{
	[Export]
	public int numCollectableFish {get; set;} = 1;
	private bool transitioning = false;
	private bool playingMinigame = false;

	private Node2D playerTextNode;
	private Node2D iceCreamTextNode;

	private int fishCollected;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Label>("UnderwaterPlayer/MinigameTime").Visible = false;
		playerTextNode = GetNode<Node2D>("UnderwaterPlayer/TextBox");
		iceCreamTextNode = GetNode<Node2D>("IceCreamAreaSprite/InteractArea/TextBox");
		fishCollected = 0;

		//disable collectable fish
		var collectableFish = GetNode<Node2D>("CollectableFish").GetChildren();
		for (int i = 0; i < collectableFish.Count; i ++)
		{
			if (collectableFish[i] is FishEnemy fish)
			{
				fish.Disable();
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override async void _Process(double delta)
	{
		if (!transitioning) {
			await NextRoomCheck();
		}
	}

	//triggered by interact area signal
	private async void StartDialogue()
	{
		if (playerTextNode is TextBox pText && iceCreamTextNode is TextBox iText)
		{
			iText.DisableInteractArea();
			await iText.showText("yo! do you want to play a minigame for coins?");
			await pText.ask("1. Yes \n2. No");
			var result = await ToSignal(pText, TextBox.SignalName.ChoiceMade);
			if (result is [Variant choice]) {
				if ((string)choice == "2") {
					await pText.showText("Maybe later.");
					iText.EnableInteractArea();
				}
				else {
					await pText.showText("Sure!");
					await iText.showText("the goal of the game is to collect as many fish as you can in the time limit.");
					await iText.showText("press enter/space near a fish to collect it.");
					await iText.showText("the fish hurt you if you get too close, so be careful!");
					StartMinigame();
				}
			}
		}
	}

	private void StartMinigame()
	{
		playingMinigame = true;
		fishCollected = 0;

		//disable regular fish
		var regularFish = GetNode<Node2D>("RegularFish").GetChildren();
		for (int i = 0; i < regularFish.Count; i ++)
		{
			if (regularFish[i] is FishEnemy fish)
			{
				fish.Disable();
			}
		}

		//enable collectable fish
		var collectableFish = GetNode<Node2D>("CollectableFish").GetChildren();
		for (int i = 0; i < collectableFish.Count; i ++)
		{
			if (collectableFish[i] is FishEnemy fish)
			{
				fish.Enable();
			}
		}

		//start timer
		var timer = GetNode<Label>("UnderwaterPlayer/MinigameTime");
		if (timer is MinigameTime mTimer)
		{
			mTimer.StartTime();
		}
	}

	//end minigame
	private async void OnTimesUp()
	{
		GetNode<CharacterBody2D>("UnderwaterPlayer").Position = new Vector2(522, 122);
		if (playerTextNode is TextBox pText && iceCreamTextNode is TextBox iText)
		{
			await iText.showText("game over!");
			await iText.showText("you got : " + fishCollected + " fish");
			await iText.showText("you get: " + fishCollected + " coins");
			//todo to-do get coins
			iText.EnableInteractArea();
		}
		
		playingMinigame = false;

		//enable regular fish
		var regularFish = GetNode<Node2D>("RegularFish").GetChildren();
		for (int i = 0; i < regularFish.Count; i ++)
		{
			if (regularFish[i] is FishEnemy fish)
			{
				fish.Enable();
			}
		}

		//disable collectable fish
		var collectableFish = GetNode<Node2D>("CollectableFish").GetChildren();
		for (int i = 0; i < collectableFish.Count; i ++)
		{
			if (collectableFish[i] is FishEnemy fish)
			{
				fish.Disable();
			}
		}
	}

	//collect fish
	private void OnFishInteract()
	{
		fishCollected ++;
		GD.Print(fishCollected);///
		//end game early if all fish collected
		if (fishCollected >= numCollectableFish)
		{
			var timer = GetNode<Label>("UnderwaterPlayer/MinigameTime");
			if (timer is MinigameTime mTimer)
			{
				mTimer.EndGame();
			}
		}
	}

	//end minigame if player dies
	private void OnPlayerHit(int hp)
	{
		if ((hp < 1 || hp == 2) && playingMinigame)
		{
			var timer = GetNode<Label>("UnderwaterPlayer/MinigameTime");
			if (timer is MinigameTime mTimer)
			{
				mTimer.EndGame();
			}
		}
	}
	
	private async Task NextRoomCheck() {
		var FaderNode = GetNode<CanvasLayer>("/root/Fader");
		Vector2 pos = GetNode<CharacterBody2D>("UnderwaterPlayer").Position;
		var GlobalScript = GetNode<GlobalSceneChange>("/root/GlobalSceneChange");
		if (pos.X < 5) {
			transitioning = true;
			if (FaderNode is Fader fader) {
				await fader.FadeIn(.7f);
			}
			await GlobalScript.ChangeRoom(new Vector2(480, 140), "box_room", false);
		}
		else if (pos.Y < 5)
		{
			transitioning = true;
			if (FaderNode is Fader fader) {
				await fader.FadeIn(.7f);
			}
			await GlobalScript.ChangeRoom(new Vector2(290, 135), "long_tube_coral_room", false);
		}
	}
}
