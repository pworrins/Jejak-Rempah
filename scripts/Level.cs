// Mengelola logika level, termasuk penyimpanan skor, pemutaran suara level selesai, dan musik latar.
using Godot;
using System;

public partial class Level : Node3D
{
    // Nomor level saat ini, dapat diatur di editor Godot untuk mengidentifikasi level.
    [Export] public int level = 1;
    // Referensi ke sistem penyimpanan untuk menyimpan skor level.
    public SaveSystem saveSystem;
    // Jalur file audio (mp3, wav, atau ogg) untuk suara yang diputar saat level selesai.
    [Export(PropertyHint.File, "*.mp3,*.wav,*.ogg")] public string LevelCompleteSound = "";
    // Jalur file audio (mp3, wav, atau ogg) untuk musik latar yang diputar selama level berjalan.
    [Export(PropertyHint.File, "*.mp3,*.wav,*.ogg")] public string BackgroundMusic = "";

    // Node untuk memutar musik latar level.
    private AudioStreamPlayer3D backgroundMusicPlayer;

    // Inisialisasi referensi ke SaveSystem dan setup audio saat level dimuat.
    public override void _Ready()
    {
        // Ambil referensi ke node SaveSystem dari root scene tree.
        saveSystem = GetTree().Root.GetNode<SaveSystem>("SaveSystem");

        // Inisialisasi musik latar jika jalur file diatur.
        if (!string.IsNullOrEmpty(BackgroundMusic))
        {
            backgroundMusicPlayer = GetNodeOrNull<AudioStreamPlayer3D>("BackgroundMusicPlayer");
            if (backgroundMusicPlayer == null)
            {
                // Buat node AudioStreamPlayer3D baru jika tidak ada.
                backgroundMusicPlayer = new AudioStreamPlayer3D();
                backgroundMusicPlayer.Name = "BackgroundMusicPlayer";
                AddChild(backgroundMusicPlayer);
            }
            // Muat dan putar musik latar, aktifkan pemutaran saat pause.
            backgroundMusicPlayer.Stream = GD.Load<AudioStream>(BackgroundMusic);
            backgroundMusicPlayer.ProcessMode = Node.ProcessModeEnum.Always;
            backgroundMusicPlayer.Play();
        }
    }

    // Menyimpan skor level ke SaveSystem dan memutar suara level selesai saat level selesai.
    public void LevelComplete()
    {
        // Ambil skor dari node UI dan simpan ke SaveSystem menggunakan nama level.
        UI ui = GetNode<UI>("UI");
        saveSystem.AddLevelScore(Name, ui.score);
        saveSystem.Save();

        // Putar suara level selesai jika jalur file diatur.
        if (!string.IsNullOrEmpty(LevelCompleteSound))
        {
            var soundPlayer = GetNodeOrNull<AudioStreamPlayer3D>("LevelCompleteSound");
            if (soundPlayer == null)
            {
                // Buat node AudioStreamPlayer3D baru jika tidak ada.
                soundPlayer = new AudioStreamPlayer3D();
                soundPlayer.Name = "LevelCompleteSound";
                AddChild(soundPlayer);
            }
            // Muat dan putar suara level selesai, aktifkan pemutaran saat pause.
            soundPlayer.Stream = GD.Load<AudioStream>(LevelCompleteSound);
            soundPlayer.ProcessMode = Node.ProcessModeEnum.Always;
            soundPlayer.Play();
        }
    }
}