	using Godot;
using System;
public partial class Player : CharacterBody3D
{
	// Kecepatan gerakan pemain (meter/detik), dapat diatur di editor Godot.
	[Export] public float SPEED = 5f;
	// Kecepatan lompatan pemain (meter/detik).
	[Export] public float JUMPVELOCITY = 8f;
	// Nilai gravitasi yang diterapkan pada pemain (meter/detik^2).
	[Export] public float GRAVITY = 8.19f;
	// Jalur ke scene main menu, digunakan untuk navigasi saat tombol Esc ditekan.
	[Export(PropertyHint.File)] public string MainMenuPath = "res://scenes/MainMenu.tscn";

	// Sudut rotasi pemain berdasarkan arah gerakan.
	public float rotation_angle = 0.0f;
	// Referensi ke node AnimationPlayer untuk mengelola animasi walk, idle, dan jump.
	public AnimationPlayer anim;
	// Referensi ke node GpuParticles3D untuk efek debu saat bergerak.
	public GpuParticles3D dust;
	// Referensi ke node Area3D untuk mendeteksi tabrakan dengan musuh.
	public Area3D EnemyHitArea;
	// Referensi ke node AudioStreamPlayer untuk suara langkah dan lompatan.
	public AudioStreamPlayer walking, jumping;
	// Vektor kecepatan pemain untuk perhitungan gerakan.
	Vector3 velocity;

	// Inisialisasi node dan pengaturan awal saat pemain masuk ke scene tree.
	public override void _Ready()
	{
		anim = GetNode<AnimationPlayer>("AnimationPlayer");
		EnemyHitArea = GetNode<Area3D>("enemy_hit_area");
		dust = GetNode<GpuParticles3D>("dust");
		walking = GetNode<AudioStreamPlayer>("sfx/walking");
		jumping = GetNode<AudioStreamPlayer>("sfx/jumping");
		// Pastikan audio awalnya tidak bermain.
		walking.StreamPaused = true;
		jumping.StreamPaused = true;
		// Izinkan pemrosesan input saat pause untuk menangani tombol P dan Esc.
		ProcessMode = Node.ProcessModeEnum.Always;
		// Pastikan audio tetap bermain saat pause.
		walking.ProcessMode = Node.ProcessModeEnum.Always;
		jumping.ProcessMode = Node.ProcessModeEnum.Always;
	}

	// Menangani input keyboard untuk pause (P) dan kembali ke main menu (Esc).
	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventKey keyEvent && keyEvent.Pressed)
		{
			if (keyEvent.Keycode == Key.P)
			{
				// Toggle pause/unpause permainan dan perbarui state visual/audio.
				GetTree().Paused = !GetTree().Paused;
				HandlePauseState();
			}
			else if (keyEvent.Keycode == Key.Escape)
			{
				// Berpindah ke main menu dengan mereset state input.
				if (!string.IsNullOrEmpty(MainMenuPath))
				{
					Input.MouseMode = Input.MouseModeEnum.Visible;
					GetTree().Paused = false;
					HandlePauseState();
					Input.FlushBufferedEvents();
					GetTree().CallDeferred("change_scene_to_file", MainMenuPath);
				}
			}
		}
	}

	// Mengelola state animasi, audio, dan partikel saat pause atau unpause.
	private void HandlePauseState()
	{
		if (GetTree().Paused)
		{
			// Saat pause, hentikan animasi dan partikel, tetapi biarkan audio tetap bermain.
			if (anim.IsPlaying())
			{
				anim.Pause();
			}
			dust.Emitting = false;
		}
		else
		{
			// Saat unpause, perbarui animasi dan partikel sesuai state pemain.
			Animate();
			dust.Emitting = IsOnFloor() && new Vector2(Velocity.X, Velocity.Z).Length() > 0;
		}
	}

	// Mengatur gerakan pemain, gravitasi, lompatan, dan rotasi setiap frame.
	public override void _Process(double delta)
	{
		if (GetTree().Paused) return; // Hentikan pemrosesan saat pause.

		velocity = Velocity;
		// Ambil input WASD untuk gerakan relatif terhadap kamera.
		Vector2 input = Input.GetVector("left", "right", "up", "down");
		Basis camBasis = GetViewport().GetCamera3D().Basis;
		Vector3 direction = camBasis * new Vector3(input.X, 0, input.Y);

		// Terapkan kecepatan berdasarkan arah input.
		velocity.X = direction.X * SPEED;
		velocity.Z = direction.Z * SPEED;

		// Terapkan gravitasi jika tidak di lantai, atau lompatan jika tombol jump ditekan.
		if (!IsOnFloor())
		{
			EnemyHitArea.Monitoring = true;
			velocity.Y -= GRAVITY * (float)delta;
		}
		else
		{
			EnemyHitArea.Monitoring = false;
			if (Input.IsActionJustPressed("jump"))
			{
				velocity.Y = JUMPVELOCITY;
				jumping.Play();
				Scale = new Vector3(0.5f, 1.5f, 0.5f); // Efek visual saat lompat.
			}
		}
		// Reset skala pemain secara bertahap setelah lompat.
		Scale = Scale.Lerp(new Vector3(1.0f, 1.0f, 1.0f), (float)delta * 4.0f);

		Velocity = velocity;

		// Rotasi pemain ke arah gerakan dengan interpolasi halus.
		if (new Vector2(Velocity.X, Velocity.Z).Length() > 0)
		{
			rotation_angle = new Vector2(Velocity.Z, Velocity.X).Angle();
			Vector3 rot = Rotation;
			rot.Y = (float)(Mathf.LerpAngle(Rotation.Y, rotation_angle, 10 * delta));
			Rotation = rot;
		}

		MoveAndSlide();
		Animate();

		// Restart level jika pemain jatuh terlalu jauh.
		if (Position.Y < -20)
		{
			Restart();
		}
	}

	// Mengelola animasi dan audio berdasarkan state pemain (bergerak, diam, atau melompat).
	public void Animate()
	{
		if (GetTree().Paused) return;

		if (IsOnFloor())
		{
			if (new Vector2(Velocity.X, Velocity.Z).Length() > 0)
			{
				anim.Play("walk");
				walking.StreamPaused = false;
				dust.Emitting = true;
			}
			else
			{
				dust.Emitting = false;
				walking.StreamPaused = true;
				anim.Play("idle");
			}
		}
		else
		{
			dust.Emitting = false;
			walking.StreamPaused = true;
			anim.Play("jump");
		}
	}

	// Menangani tabrakan dengan musuh, memicu kematian musuh dan lompatan pemain.
	public void HitEnemy(Enemy body)
	{
		if (body.HasMethod("Die"))
		{
			body.Die();
		}
		velocity.Y = JUMPVELOCITY;
		Velocity = velocity;
	}

	// Memuat ulang scene saat ini untuk restart level.
	public void Restart()
	{
		GetTree().ReloadCurrentScene();
	}
}