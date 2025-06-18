using Godot;
using System;

public partial class Lada : Area3D
{
	// Signal untuk ketika lada dikumpulkan dengan nilai poin
	[Signal]
	public delegate void LadaCollectedEventHandler(int points);

	public CollisionShape3D col;
	public AudioStreamPlayer spice_fx;

	[Export]
	public float spin_speed = 4.5f;
	
	[Export]
	public int point_value = 3; // Total poin jika mengenai lada = 3
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		spice_fx = GetNode<AudioStreamPlayer>("spice_fx");
		col = GetNode<CollisionShape3D>("col");
		
		// PENTING: Hubungkan signal AreaEntered ke method Collected
		AreaEntered += OnAreaEntered;
		BodyEntered += OnBodyEntered;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		RotateY(spin_speed * (float)delta);
	}

	public void SetColOff()
	{
		col.Disabled = true;
	}

	// Method yang dipanggil ketika Player masuk ke area
	public void OnBodyEntered(Node3D body)
	{
		if (body is Player player)
		{
			Collected(player);
		}
	}
	
	public void OnAreaEntered(Area3D area)
	{
		if (area.GetParent() is Player player)
		{
			Collected(player);
		}
	}

	public void Collected(Player body)
	{
		// Prevent node queue error
		CallDeferred("SetColOff");
		
		// Emit signal dengan nilai poin
		EmitSignal("LadaCollected", point_value);
		
		// Debug print untuk memastikan signal dipanggil
		GD.Print($"Lada collected! Points: {point_value}");
		
		// Hide so it seems like it was removed
		Hide();
		
		// Create tween for smooth collection effect
		Tween tw = CreateTween();
		
		// Play audio and set delay to remove afterwards
		tw.TweenCallback(Callable.From(() => spice_fx.Play()));
		tw.TweenCallback(Callable.From(() => QueueFree())).SetDelay(2f);
	}

	// Method untuk mengatur nilai poin secara dinamis
	public void SetPointValue(int newValue)
	{
		point_value = newValue;
	}
	
	// Method untuk mendapatkan nilai poin
	public int GetPointValue()
	{
		return point_value;
	}
}