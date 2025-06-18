// using Godot;
// using System;
// using System.IO;

// public partial class LevelComplete : Area3D
// {
// 	// Called when the node enters the scene tree for the first time.
// 	[Export(PropertyHint.File)]
// 	public String NextLevelPath = "";

// 	// public void Complete(Player body)
// 	// {
// 	// 	if(NextLevelPath != "")
// 	// 	{
// 	// 		((Level) GetParent()).LevelComplete();

// 	// 		// Reset input state sebelum pindah scene
// 	// 		Input.MouseMode = Input.MouseModeEnum.Visible;
// 	// 		GetTree().Paused = false;
// 	// 		Input.FlushBufferedEvents();

// 	// 		// Gunakan CallDeferred untuk mencegah error physics callback
// 	// 		GetTree().CallDeferred("change_scene_to_file", NextLevelPath);
// 	// 	}
// 	// 	else
// 	// 	{
// 	// 		GD.Print("Path not set");
// 	// 	}
// 	// }


// 	// Alternative method: Jika Anda ingin memisahkan logic
// 	private void ChangeSceneDeferred()
// 	{
// 		if (NextLevelPath != "")
// 		{
// 			GetTree().ChangeSceneToFile(NextLevelPath);
// 		}
// 	}

// 	// Method alternatif untuk memanggil Complete dengan deferred
// 	public void CompleteDeferred(Player body)
// 	{
// 		if(NextLevelPath != "")
// 		{
// 			((Level) GetParent()).LevelComplete();
// 			CallDeferred("ChangeSceneDeferred");
// 		}
// 		else
// 		{
// 			GD.Print("Path not set");
// 		}
// 	}
// }

using Godot;
  using System;
  using System.IO;

  public partial class LevelComplete : Area3D
  {
      [Export(PropertyHint.File)]
      public String NextLevelPath = "";
      [Export(PropertyHint.File, "*.mp3,*.wav,*.ogg")] // Corrected syntax
      public string LevelCompleteSound = ""; // Override suara dari Level jika perlu

      public void Complete(Player body)
      {
          if (NextLevelPath != "")
          {
              ((Level)GetParent()).LevelComplete();
              
              // Mainkan suara level complete dari Level jika ada, atau gunakan yang di sini
              if (!string.IsNullOrEmpty(LevelCompleteSound))
              {
                  var soundPlayer = GetNodeOrNull<AudioStreamPlayer3D>("LevelCompleteSound");
                  if (soundPlayer == null)
                  {
                      soundPlayer = new AudioStreamPlayer3D();
                      soundPlayer.Name = "LevelCompleteSound";
                      AddChild(soundPlayer);
                  }
                  soundPlayer.Stream = GD.Load<AudioStream>(LevelCompleteSound);
                  soundPlayer.Play();
              }

              // Reset input state sebelum pindah scene
              Input.MouseMode = Input.MouseModeEnum.Visible;
              GetTree().Paused = false;
              Input.FlushBufferedEvents();
              
              GetTree().CallDeferred("change_scene_to_file", NextLevelPath);
          }
          else
          {
              GD.Print("Path not set");
          }
      }
  }