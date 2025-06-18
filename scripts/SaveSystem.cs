// using Godot;
// using Godot.Collections;
// using System;

// public partial class SaveSystem : Node
// {
// 	public Dictionary player_data = new Dictionary();

// 	//Save file path dedfault storage paths
// 	//Linux: .local/share/godot/app_userdata/app_name/filename
// 	//Windows: %APPDATA%/godot/appname/filename
// 	public String save_path = "user://CsharpGame.sav";
// 	// Called when the node enters the scene tree for the first time.
// 	public override void _Ready()
// 	{
// 		//load saved data if any
// 		Load();
// 	}

// 	//adds level score
// 	public void AddLevelScore(String level, int score){
// 		player_data[level] = score;
// 	}
// 	//save all data
// 	public void Save(){
// 		FileAccess file = FileAccess.Open(save_path,FileAccess.ModeFlags.Write);
// 		file.StoreVar(player_data);
// 	}

// 	public void Load(){
// 		//Check if file exist then load it otherwize set a new Dictionary
// 		if (FileAccess.FileExists(save_path)){
// 			FileAccess file = FileAccess.Open(save_path,FileAccess.ModeFlags.Read);
// 			player_data = (Dictionary)file.GetVar();
// 		}else{
// 			player_data = new Dictionary();

// 		}
// 		GD.Print(player_data);
// 	}
// }
using Godot;
using Godot.Collections;
using System;

public partial class SaveSystem : Node
{
    public Dictionary<string, Variant> player_data = new Dictionary<string, Variant>();
    private string save_file_path = "C:/Users/USER/OneDrive - Politeknik Negeri Bandung/Documents/godot-c-sharp-series-main/data/save_game.dat";

    public override void _Ready()
    {
        // Initialize default scores untuk 4 level
        InitializeDefaultScores();
        Load();
    }

    private void InitializeDefaultScores()
    {
        // Set default scores jika belum ada
        if (!player_data.ContainsKey("Level1"))
            player_data["Level1"] = 0;
        if (!player_data.ContainsKey("Level2"))
            player_data["Level2"] = 0;
        if (!player_data.ContainsKey("Level3"))
            player_data["Level3"] = 0;
        if (!player_data.ContainsKey("Level4"))
            player_data["Level4"] = 0;
    }

    public void AddLevelScore(string levelName, int score)
    {
        // Pastikan level name valid
        if (levelName == "Level1" || levelName == "Level2" || levelName == "Level3" || levelName == "Level4")
        {
            // Hanya update score jika lebih tinggi dari sebelumnya
            int currentScore = player_data.ContainsKey(levelName) ? (int)player_data[levelName] : 0;
            if (score > currentScore)
            {
                player_data[levelName] = score;
                GD.Print($"New high score for {levelName}: {score}");
            }
            else
            {
                GD.Print($"Score {score} for {levelName} not higher than current best: {currentScore}");
            }
        }
        else
        {
            GD.PrintErr($"Invalid level name: {levelName}");
        }
    }

    public int GetLevelScore(string levelName)
    {
        return player_data.ContainsKey(levelName) ? (int)player_data[levelName] : 0;
    }

    public int GetTotalScore()
    {
        int total = 0;
        total += GetLevelScore("Level1");
        total += GetLevelScore("Level2");
        total += GetLevelScore("Level3");
        total += GetLevelScore("Level4");
        return total;
    }

    public void Save()
    {
        var save_file = FileAccess.Open(save_file_path, FileAccess.ModeFlags.Write);
        if (save_file != null)
        {
            save_file.StoreString(Json.Stringify(player_data));
            save_file.Close();
            GD.Print("Game saved successfully!");
        }
        else
        {
            GD.PrintErr("Failed to save game!");
        }
    }

    public void Load()
    {
        if (FileAccess.FileExists(save_file_path))
        {
            var save_file = FileAccess.Open(save_file_path, FileAccess.ModeFlags.Read);
            if (save_file != null)
            {
                string json_string = save_file.GetAsText();
                save_file.Close();

                var json = new Json();
                var parse_result = json.Parse(json_string);
                
                if (parse_result == Error.Ok)
                {
                    var loaded_data = json.GetData().AsGodotDictionary<string, Variant>();
                    if (loaded_data != null)
                    {
                        player_data = loaded_data;
                        GD.Print("Game loaded successfully!");
                        
                        // Pastikan semua level ada dalam data
                        InitializeDefaultScores();
                    }
                }
                else
                {
                    GD.PrintErr("Failed to parse save file!");
                    InitializeDefaultScores();
                }
            }
        }
        else
        {
            GD.Print("No save file found, using default values.");
            InitializeDefaultScores();
        }
    }

    public void ResetAllScores()
    {
        player_data["Level1"] = 0;
        player_data["Level2"] = 0;
        player_data["Level3"] = 0;
        player_data["Level4"] = 0;
        Save();
        GD.Print("All scores reset!");
    }

    // Method untuk unlock level berdasarkan score
    public bool IsLevelUnlocked(int levelNumber)
    {
        if (levelNumber == 1) return true; // Level 1 selalu unlocked
        
        // Level selanjutnya unlock jika level sebelumnya sudah ada scorenya
        string previousLevel = $"Level{levelNumber - 1}";
        return GetLevelScore(previousLevel) > 0;
    }
}