using Godot;
using System;

public partial class Pala : Area3D
{
	// Signal untuk ketika pala dikumpulkan dengan nilai poin
	[Signal]
	public delegate void PalaCollectedEventHandler(int points);

	public CollisionShape3D col;
	public AudioStreamPlayer spice_fx;

	[Export]
	public float spin_speed = 3.0f;
	
	[Export]
	public int point_value = 7; // Total poin jika mengenai pala = 7
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		spice_fx = GetNode<AudioStreamPlayer>("spice_fx");
		col = GetNode<CollisionShape3D>("col");
		
		// PENTING: Hanya hubungkan salah satu sinyal!
        // Jika player Anda adalah CharacterBody3D atau RigidBody3D, gunakan BodyEntered.
        // Jika player Anda adalah Area3D, gunakan AreaEntered.
		// Asumsi Player adalah Body, maka kita gunakan BodyEntered.
		//AreaEntered += OnAreaEntered; // Komentari atau hapus baris ini
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
	
	// Method OnAreaEntered tidak akan terpanggil jika koneksinya dihapus di _Ready()
	// Anda bisa menghapusnya sepenuhnya atau membiarkannya jika Anda hanya mengomentari koneksinya.
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
		EmitSignal("PalaCollected", point_value);
		
		// Debug print untuk memastikan signal dipanggil
		GD.Print($"Pala collected! Points: {point_value}");
		
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