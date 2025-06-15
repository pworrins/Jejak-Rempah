using Godot;
using System;

public partial class GameCam : Node3D
{
    [Export] public Node3D target;
    [Export] public Node3D pivot;
    [Export] public Camera3D camera;

    private Vector2 rotation = Vector2.Zero;
    private float mouseSensitivity = 0.003f;
    private float zoomSpeed = 2f;
    private float minZoom = 2f;
    private float maxZoom = 20f;

    private bool isLooking = true;

    public override void _Ready()
    {
        if (target == null)
            target = GetParent().GetNode<Node3D>("character");
        if (pivot == null)
            pivot = GetNode<Node3D>("Pivot");
        if (camera == null)
            camera = pivot.GetNode<Camera3D>("Camera3D");

        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _Process(double delta)
    {
        GlobalPosition = target.GlobalPosition;
    }

    public override void _Input(InputEvent @event)
    {
        // Tangani rotasi dengan mouse
        if (Input.MouseMode == Input.MouseModeEnum.Captured && @event is InputEventMouseMotion motion)
        {
            rotation.X -= motion.Relative.Y * mouseSensitivity;
            rotation.Y -= motion.Relative.X * mouseSensitivity;

            rotation.X = Mathf.Clamp(rotation.X, Mathf.DegToRad(-80), Mathf.DegToRad(80));

            Rotation = new Vector3(0, rotation.Y, 0);
            pivot.Rotation = new Vector3(rotation.X, 0, 0);
        }

        // Tangani scroll untuk zoom
        if (@event is InputEventMouseButton mouseBtn && mouseBtn.Pressed)
        {
            if (mouseBtn.ButtonIndex == MouseButton.WheelUp)
            {
                Vector3 camPos = camera.Position;
                camPos.Z = Mathf.Clamp(camPos.Z - zoomSpeed, -maxZoom, -minZoom);
                camera.Position = camPos;
            }
            else if (mouseBtn.ButtonIndex == MouseButton.WheelDown)
            {
                Vector3 camPos = camera.Position;
                camPos.Z = Mathf.Clamp(camPos.Z + zoomSpeed, -maxZoom, -minZoom);
                camera.Position = camPos;
            }
            else if (mouseBtn.ButtonIndex == MouseButton.Right)
            {
                // Tekan klik kanan untuk toggle mouse mode
                if (Input.MouseMode == Input.MouseModeEnum.Visible)
                    Input.MouseMode = Input.MouseModeEnum.Captured;
                else
                    Input.MouseMode = Input.MouseModeEnum.Visible;
            }
        }
    }
}
