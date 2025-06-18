// Mengelola perilaku musuh dengan state machine untuk idle, menyerang, dan mati.
using Godot;
using Godot.Collections;
using System;

public partial class Enemy : CharacterBody3D
{
    // Kecepatan gerakan musuh saat menyerang (meter/detik).
    public float SPEED = 4.0f;
    // Jarak maksimum untuk mendeteksi pemain dan memulai serangan (meter).
    public float ATTACKRANGE = 10.0f;
    // Kecepatan lompatan musuh (meter/detik).
    public float JumpVelocity = 2.0f;
    // Nilai gravitasi yang diterapkan pada musuh (meter/detik^2).
    public float GRAVITY = 9.85f;

    // Referensi ke node AnimationPlayer untuk animasi walk dan idle.
    public AnimationPlayer anim;
    // Referensi ke node pemain sebagai target musuh.
    public Node3D target;
    // Sudut rotasi musuh berdasarkan arah gerakan.
    public float rotation_direction = 0.0f;
    // Vektor kecepatan musuh untuk perhitungan gerakan.
    public Vector3 velocity;

    // Enum untuk state musuh: idle, menyerang, berjuang, atau mati.
    public enum State
    {
        IDLE,
        ATTACK,
        STRUGGLE,
        DIE
    }

    // State saat ini dari musuh.
    public State CurrentState;

    // Inisialisasi referensi node dan state awal saat musuh masuk ke scene tree.
    public override void _Ready()
    {
        CurrentState = State.IDLE;

        // Coba ambil node target (pemain) dari scene tree.
        try
        {
            target = GetParent()?.GetParent()?.GetNode<Node3D>("character");
        }
        catch (Exception )
        {
            // Tangani error jika target tidak ditemukan.
        }

        // Ambil node AnimationPlayer untuk animasi.
        try
        {
            anim = GetNode<AnimationPlayer>("AnimationPlayer");
        }
        catch
        {
            // Tangani error jika AnimationPlayer tidak ditemukan.
        }
    }

    // Memperbarui perilaku musuh berdasarkan state setiap frame fisika.
    public override void _PhysicsProcess(double delta)
    {
        if (GetTree().Paused) return; // Hentikan pemrosesan saat pause.

        switch (CurrentState)
        {
            case State.IDLE:
                UpdateIdle();
                break;
            case State.ATTACK:
                UpdateAttack(delta);
                break;
            case State.DIE:
                UpdateDie();
                break;
        }

        // Terapkan gravitasi jika musuh tidak di lantai.
        velocity = Velocity;
        if (!IsOnFloor())
            velocity.Y -= GRAVITY * (float)delta;

        // Rotasi musuh ke arah gerakan dengan interpolasi halus.
        if (new Vector2(Velocity.X, Velocity.Z).Length() > 0.0)
        {
            rotation_direction = new Vector2(Velocity.Z, Velocity.X).Angle();
            Vector3 rot = Rotation;
            rot.Y = (float)(Mathf.LerpAngle(Rotation.Y, rotation_direction, delta * 10));
            Rotation = rot;
        }

        Velocity = velocity;
        Animate();
    }

    // Memperbarui state idle, memeriksa jarak ke pemain untuk memulai serangan.
    private void UpdateIdle()
    {
        if (target != null && Position.DistanceTo(target.Position) < ATTACKRANGE)
        {
            CurrentState = State.ATTACK;
        }
    }

    // Memperbarui state serangan, menggerakkan musuh menuju pemain.
    private void UpdateAttack(double delta)
    {
        if (target == null) return;

        // Hitung arah menuju pemain pada sumbu X dan Z.
        Vector3 t = target.Position;
        t.Y = Position.Y;
        Vector3 direction = Position.DirectionTo(t);

        // Terapkan kecepatan berdasarkan arah menuju pemain.
        velocity = Velocity;
        velocity.X = direction.X * SPEED;
        velocity.Z = direction.Z * SPEED;
        Velocity = velocity;

        MoveAndSlide();
        CheckforPlayer();
    }

    // Memeriksa tabrakan dengan pemain untuk memicu restart level.
    public void CheckforPlayer()
    {
        for (int i = 0; i < GetSlideCollisionCount(); i++)
        {
            KinematicCollision3D c = GetSlideCollision(i);
            GodotObject co = c.GetCollider();

            if (co is Player player && player.IsOnFloor())
            {
                // Muat ulang scene saat ini jika pemain di lantai bertabrakan dengan musuh.
                var tree = GetTree();
                if (tree != null)
                {
                    tree.ReloadCurrentScene();
                }
            }
        }
    }

    // Mengelola animasi musuh berdasarkan kecepatan gerakan.
    public void Animate()
    {
        if (anim == null) return;

        if (anim.HasAnimation("walk") && new Vector2(Velocity.X, Velocity.Z).Length() > 0.0f)
        {
            anim.Play("walk");
        }
        else if (anim.HasAnimation("idle_leg-left"))
        {
            anim.Play("idle_leg-left");
        }
    }

    // Memperbarui state kematian dengan animasi skala dan penghapusan node.
    public void UpdateDie()
    {
        // Animasi kematian dengan tween untuk efek skala sebelum dihapus.
        Tween tw = CreateTween();
        tw.TweenProperty(this, "scale", new Vector3(1.5f, 0.25f, 1.5f), 0.2f);
        tw.TweenCallback(Callable.From(() => QueueFree()));
    }

    // Mengubah state musuh ke kematian.
    public void Die()
    {
        CurrentState = State.DIE;
    }
}