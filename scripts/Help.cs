using Godot;

public partial class Help : Control
{
    
    public void BackToMainMenu()
    {
        GD.Print("BackToMainMenu() dipanggil - Tombol diklik!");
		GetTree().ChangeSceneToFile("res://scenes/MainMenu.tscn");
	
    }
}