// using Godot;
// using Godot.Collections;
// using System;

// public partial class UI : CanvasLayer
// {
// 	public Label score_lbl;
// 	public int score = 0;

// 	public override void _Ready()
// 	{
// 		score_lbl = GetNode<Label>("hb/score_lbl");

// 		// Get all the nodes in the coin group
// 		Array<Node> coins = GetTree().GetNodesInGroup("coins");
// 		Array<Node> rempah = GetTree().GetNodesInGroup("rempah");

// 		// Connect the CoinCollected signal of each coin to AddScore function 
// 		for (int i = 0; i < coins.Count; i++)
// 		{
// 			Coin n = (Coin)coins[i];
// 			n.CoinCollected += OnCoinCollected; // 1 point untuk coin
// 		}

// 		// Connect signals untuk semua rempah-rempah
// 		for (int i = 0; i < rempah.Count; i++)
// 		{
// 			// Cek tipe collectible dan connect ke method yang sesuai
// 			if (rempah[i] is Jahe jahe)
// 			{
// 				jahe.GingerCollected += OnSpiceCollected; // 2 points untuk jahe
// 			}
// 			else if (rempah[i] is Lada lada)
// 			{
// 				lada.LadaCollected += OnSpiceCollected; // 3 points untuk lada
// 			}
// 			else if (rempah[i] is Cengkeh cengkeh)
// 			{
// 				cengkeh.CengkehCollected += OnSpiceCollected; // 5 points untuk cengkeh
// 			}
// 			else if (rempah[i] is Pala pala)
// 			{
// 				pala.PalaCollected += OnSpiceCollected; // 7 points untuk pala
// 			}
// 			else if (rempah[i] is KayuManis kayuManis)
// 			{
// 				kayuManis.KayuManisCollected += OnSpiceCollected; // 10 points untuk kayu manis
// 			}
// 		}
// 	}

// 	public void AddScore(int points = 1)
// 	{
// 		score += points;
// 		score_lbl.Text = "x " + score.ToString();
		
// 		// Debug print untuk memastikan AddScore dipanggil
// 		GD.Print($"Score updated: +{points}, Total: {score}");
// 	}

// 	// Method khusus untuk coin (1 poin)
// 	public void OnCoinCollected()
// 	{
// 		AddScore(1);
// 		GD.Print("Coin collected: +1 point");
// 	}

// 	// Method khusus untuk semua rempah (sesuai nilai yang dikirim)
// 	public void OnSpiceCollected(int points)
// 	{
// 		AddScore(points);
// 		GD.Print($"Spice collected: +{points} points");
// 	}
// }
using Godot;
using Godot.Collections;
using System;

public partial class UI : CanvasLayer
{
	public Label score_lbl;
	public Label pauseLabel;
	public int score = 0;

	public override void _Ready()
	{
		score_lbl = GetNode<Label>("hb/score_lbl");
		pauseLabel = GetNodeOrNull<Label>("pause_label");
		if (pauseLabel == null)
		{
			GD.PrintErr("pause_label not found in scene! Please add a Label node named 'pause_label'.");
		}
		else
		{
			pauseLabel.Hide();
		}

		// Pastikan CanvasLayer selalu memproses, bahkan saat pause
		ProcessMode = Node.ProcessModeEnum.Always;

		Array<Node> coins = GetTree().GetNodesInGroup("coins");
		Array<Node> rempah = GetTree().GetNodesInGroup("rempah");

		for (int i = 0; i < coins.Count; i++)
		{
			Coin n = (Coin)coins[i];
			n.CoinCollected += OnCoinCollected;
		}

		for (int i = 0; i < rempah.Count; i++)
		{
			if (rempah[i] is Jahe jahe)
			{
				jahe.GingerCollected += OnSpiceCollected;
			}
			else if (rempah[i] is Lada lada)
			{
				lada.LadaCollected += OnSpiceCollected;
			}
			else if (rempah[i] is Cengkeh cengkeh)
			{
				cengkeh.CengkehCollected += OnSpiceCollected;
			}
			else if (rempah[i] is Pala pala)
			{
				pala.PalaCollected += OnSpiceCollected;
			}
			else if (rempah[i] is KayuManis kayuManis)
			{
				kayuManis.KayuManisCollected += OnSpiceCollected;
			}
		}
	}

	public override void _Process(double delta)
	{
		if (pauseLabel != null)
		{
			pauseLabel.Visible = GetTree().Paused;
		}
	}

	public void AddScore(int points = 1)
	{
		score += points;
		score_lbl.Text = "x " + score.ToString();
		GD.Print($"Score updated: +{points}, Total: {score}");
	}

	public void OnCoinCollected()
	{
		AddScore(1);
		GD.Print("Coin collected: +1 point");
	}

	public void OnSpiceCollected(int points)
	{
		AddScore(points);
		GD.Print($"Spice collected: +{points} points");
	}
}