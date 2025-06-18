// Mengelola kamera yang mengikuti pemain dengan rotasi berbasis mouse dan zoom menggunakan scroll.
using Godot;
using System;

public partial class GameCam : Node3D
{
    // Referensi ke node pemain sebagai target kamera.
    [Export] public Node3D target;
    // Referensi ke node pivot untuk rotasi vertikal kamera.
    [Export] public Node3D pivot;
    // Referensi ke node Camera3D untuk rendering tampilan.
    [Export] public Camera3D camera;

    // Rotasi kamera (X untuk vertikal, Y untuk horizontal) dalam radian.
    private Vector2 rotation = Vector2.Zero;
    // Sensitivitas gerakan mouse untuk rotasi kamera.
    private float mouseSensitivity = 0.003f;
    // Kecepatan zoom saat menggunakan scroll mouse.
    private float zoomSpeed = 2f;
    // Jarak zoom minimum dari pivot.
    private float minZoom = 2f;
    // Jarak zoom maksimum dari pivot.
    private float maxZoom = 20f;

    // Status apakah kamera sedang dikontrol oleh mouse.
    private bool isLooking = true;

    // Inisialisasi referensi node dan pengaturan awal kamera saat masuk ke scene tree.
    public override void _Ready()
    {
        // Set target ke node karakter jika belum diatur di editor.
        if (target == null)
            target = GetParent().GetNode<Node3D>("character");
        // Set pivot ke node Pivot jika belum diatur.
        if (pivot == null)
            pivot = GetNode<Node3D>("Pivot");
        // Set kamera ke node Camera3D di bawah Pivot jika belum diatur.
        if (camera == null)
            camera = pivot.GetNode<Camera3D>("Camera3D");

        // Kunci kursor mouse untuk kontrol kamera.
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    // Memperbarui posisi kamera untuk mengikuti target setiap frame.
    public override void _Process(double delta)
    {
        GlobalPosition = target.GlobalPosition;
    }

    // Menangani input mouse untuk rotasi kamera dan zoom.
    public override void _Input(InputEvent @event)
    {
        // Rotasi kamera berdasarkan gerakan mouse saat kursor terkunci.
        if (Input.MouseMode == Input.MouseModeEnum.Captured && @event is InputEventMouseMotion motion)
        {
            // Perbarui rotasi berdasarkan gerakan mouse dengan sensitivitas.
            rotation.X -= motion.Relative.Y * mouseSensitivity;
            rotation.Y -= motion.Relative.X * mouseSensitivity;

            // Batasi rotasi vertikal untuk mencegah kamera terbalik.
            rotation.X = Mathf.Clamp(rotation.X, Mathf.DegToRad(-80), Mathf.DegToRad(80));

            // Terapkan rotasi horizontal ke node ini dan vertikal ke pivot.
            Rotation = new Vector3(0, rotation.Y, 0);
            pivot.Rotation = new Vector3(rotation.X, 0, 0);
        }

        // Tangani zoom dan toggle kursor dengan tombol mouse.
        if (@event is InputEventMouseButton mouseBtn && mouseBtn.Pressed)
        {
            if (mouseBtn.ButtonIndex == MouseButton.WheelUp)
            {
                // Zoom in dengan mengurangi jarak kamera, batasi dengan minZoom.
                Vector3 camPos = camera.Position;
                camPos.Z = Mathf.Clamp(camPos.Z - zoomSpeed, -maxZoom, -minZoom);
                camera.Position = camPos;
            }
            else if (mouseBtn.ButtonIndex == MouseButton.WheelDown)
            {
                // Zoom out dengan menambah jarak kamera, batasi dengan maxZoom.
                Vector3 camPos = camera.Position;
                camPos.Z = Mathf.Clamp(camPos.Z + zoomSpeed, -maxZoom, -minZoom);
                camera.Position = camPos;
            }
            else if (mouseBtn.ButtonIndex == MouseButton.Right)
            {
                // Toggle kursor antara terkunci dan terlihat saat klik kanan.
                Input.MouseMode = Input.MouseMode == Input.MouseModeEnum.Visible
                    ? Input.MouseModeEnum.Captured
                    : Input.MouseModeEnum.Visible;
            }
        }
    }
}