using Godot;
using System;

public partial class MinigameTime : Label
{
	[Export]
	public int seconds {get; set;} = 20;
	[Signal]
	public delegate void TimesUpEventHandler();

	private double time;
	private Boolean timing;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		time = 0;
		timing = false;
		Visible = false;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (timing)
		{
			time -= delta;
			Text = "Time: " + (int) time + "s";
			if (time <= 0)
			{
				timing = false;
				Text = "Times up! " + 0;
				EmitSignal(SignalName.TimesUp);
			}
		}
	}

	public void StartTime()
	{
		time = seconds;
		timing = true;
		Visible = true;
	}

	public void EndGame() //for ending the game early
	{
		timing = false;
		Visible = false;
	}
}
