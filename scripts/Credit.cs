using Godot;
using System;

public partial class Credit : Control
{
     public void BackToMainMenu()
    {
        GD.Print("BackToMainMenu() dipanggil - Tombol diklik!");
		GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
	
    }
}
