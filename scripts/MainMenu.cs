using Godot;
using System;

public partial class MainMenu : Control
{
    public SaveSystem saveSystem;
    [Export(PropertyHint.File)]
    public String LevelOnePath = "";
    [Export(PropertyHint.File)]
    public String SettingsPath = "";
    [Export(PropertyHint.File)]
    public String HelpPath = "res://scenes/Help.tscn"; // Properti baru untuk jalur scene Help
    [Export(PropertyHint.File)]
    public String CreditPath = "res://scenes/Credit.tscn"; // Properti baru untuk jalur scene Credit
    public Label scores_lbl;
    public Label totalScoreLbl;
    public bool showhide = false;
    public AudioStreamPlayer3D clickSound;

    public override void _Ready()
    {
        GD.Print("MainMenu _Ready() dipanggil");
        
        // RESET SEMUA STATE DULU
        GetTree().Paused = false;
        Input.MouseMode = Input.MouseModeEnum.Visible;
        
        // Bersihkan buffer input
        Input.FlushBufferedEvents();
        
        // Siapkan referensi
        saveSystem = GetTree().Root.GetNode<SaveSystem>("SaveSystem");
        scores_lbl = GetNode<Label>("scores");
        totalScoreLbl = GetNode<Label>("total_score");
        clickSound = GetNodeOrNull<AudioStreamPlayer3D>("ClickSound");
        scores_lbl.Hide();
        
        if (clickSound == null)
        {
            GD.PrintErr("Node ClickSound tidak ditemukan di scene!");
        }
        else
        {
            clickSound.StreamPaused = false; // Pastikan audio tidak paused
            GD.Print("Node ClickSound ditemukan dan siap.");
        }
        
        // PENTING: Aktifkan semua mode proses
        SetProcessInput(true);
        SetProcessUnhandledInput(true);
        ProcessMode = Node.ProcessModeEnum.Always;
        
        // Paksa aktifkan filter mouse untuk semua tombol
        CallDeferred(nameof(EnableAllButtons));
        
        // Perbarui total skor
        totalScoreLbl.Text = "Total System: Skor: " + saveSystem.GetTotalScore().ToString();
        
        GD.Print("Pengaturan MainMenu selesai");
    }
    
    private void EnableAllButtons()
    {
        var buttons = FindChildren("*", "Button", true, false);
        foreach (Node child in buttons)
        {
            if (child is Button button)
            {
                button.Disabled = false;
                button.MouseFilter = Control.MouseFilterEnum.Pass;
                button.ProcessMode = Node.ProcessModeEnum.Always;
                GD.Print($"Tombol {button.Name} diaktifkan");
            }
        }
        
        MouseFilter = Control.MouseFilterEnum.Pass;
        ProcessMode = Node.ProcessModeEnum.Always;
    }

    public void LoadScores()
    {
        GD.Print("LoadScores() dipanggil");
        
        int l1 = saveSystem.player_data.ContainsKey("Level1") ? (int)saveSystem.player_data["Level1"] : 0;
        int l2 = saveSystem.player_data.ContainsKey("Level2") ? (int)saveSystem.player_data["Level2"] : 0;
        int l3 = saveSystem.player_data.ContainsKey("Level3") ? (int)saveSystem.player_data["Level3"] : 0;
        int l4 = saveSystem.player_data.ContainsKey("Level4") ? (int)saveSystem.player_data["Level4"] : 0;
        
        scores_lbl.Text = "SKOR \n";
        scores_lbl.Text += "Level 1: " + l1.ToString() + "\n";
        scores_lbl.Text += "Level 2: " + l2.ToString() + "\n";
        scores_lbl.Text += "Level 3: " + l3.ToString() + "\n";
        scores_lbl.Text += "Level 4: " + l4.ToString() + "\n";
        
        // Tampilkan total skor
        int totalScore = l1 + l2 + l3 + l4;
        scores_lbl.Text += "Total: " + totalScore.ToString() + "\n";
        
        // Tampilkan level yang terbuka
        scores_lbl.Text += "Level Terbuka: ";
        for (int i = 1; i <= 4; i++)
        {
            if (saveSystem.IsLevelUnlocked(i))
                scores_lbl.Text += $"Level {i}, ";
        }
    }

    public void StartGame()
    {
        GD.Print("StartGame() dipanggil - Tombol diklik!");
        if (string.IsNullOrEmpty(LevelOnePath))
        {
            GD.PrintErr("LevelOnePath belum diatur!");
            return;
        }
        
        GD.Print($"Berpindah ke scene: {LevelOnePath}");
        if (clickSound != null && clickSound.Stream != null)
        {
            clickSound.Play(); // Pastikan stream ada sebelum diputar
        }
        GetTree().CallDeferred("change_scene_to_file", LevelOnePath);
    }

    public void ShowHideScores()
    {
        GD.Print("ShowHideScores() dipanggil - Tombol diklik!");
        if (clickSound != null && clickSound.Stream != null) clickSound.Play();
        if (showhide == false)
        {
            LoadScores();
            scores_lbl.Show();
            showhide = true;
        }
        else
        {
            scores_lbl.Hide();
            showhide = false;
        }
    }
    
    public void QuitGame()
    {
        GD.Print("QuitGame() dipanggil - Tombol diklik!");
        if (clickSound != null && clickSound.Stream != null) clickSound.Play();
        GetTree().Quit();
    }
    
    public void OpenSettings()
    {
        GD.Print("OpenSettings() dipanggil - Tombol diklik!");
        if (!string.IsNullOrEmpty(SettingsPath))
        {
            if (clickSound != null && clickSound.Stream != null) clickSound.Play();
            GetTree().CallDeferred("change_scene_to_file", SettingsPath);
        }
        else
        {
            GD.PrintErr("SettingsPath belum diatur!");
        }
    }
    
    // Metode baru untuk menu Help
    public void OpenHelp()
    {
        GD.Print("OpenHelp() dipanggil - Tombol diklik!");
        if (!string.IsNullOrEmpty(HelpPath))
        {
            if (clickSound != null && clickSound.Stream != null) clickSound.Play();
            GetTree().CallDeferred("change_scene_to_file", HelpPath);
        }
        else
        {
            GD.PrintErr("HelpPath belum diatur!");
        }
    }
    
    // Metode baru untuk menu Credit
    public void OpenCredit()
    {
        GD.Print("OpenCredit() dipanggil - Tombol diklik!");
        if (!string.IsNullOrEmpty(CreditPath))
        {
            if (clickSound != null && clickSound.Stream != null) clickSound.Play();
            GetTree().CallDeferred("change_scene_to_file", CreditPath);
        }
        else
        {
            GD.PrintErr("CreditPath belum diatur!");
        }
    }
    
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseEvent && mouseEvent.Pressed)
        {
            GD.Print($"Mouse diklik di: {mouseEvent.Position}");
            if (clickSound != null && clickSound.Stream != null) clickSound.Play();
        }
        
        if (@event is InputEventKey keyEvent && keyEvent.Pressed)
        {
            GD.Print($"Tombol ditekan: {keyEvent.Keycode}");
            
            if (keyEvent.Keycode == Key.Enter || keyEvent.Keycode == Key.Space)
            {
                GD.Print("Memulai permainan melalui keyboard");
                StartGame();
            }
            
            if (keyEvent.Keycode == Key.Escape)
            {
                GD.Print("Keluar dari permainan melalui keyboard");
                QuitGame();
            }
            
            // Opsional: Tambahkan pintasan keyboard untuk Help dan Credit
            if (keyEvent.Keycode == Key.H)
            {
                GD.Print("Membuka Help melalui keyboard");
                OpenHelp();
            }
            
            if (keyEvent.Keycode == Key.C)
            {
                GD.Print("Membuka Credit melalui keyboard");
                OpenCredit();
            }
        }
    }
}