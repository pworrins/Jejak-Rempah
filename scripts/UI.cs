using Godot;
using Godot.Collections;
using System;

public partial class UI : CanvasLayer
{
	public Label score_lbl;
	public int score = 0;

	public override void _Ready()
	{
		score_lbl = GetNode<Label>("hb/score_lbl");

		// Get all the nodes in the coin group
		Array<Node> coins = GetTree().GetNodesInGroup("coins");
		Array<Node> rempah = GetTree().GetNodesInGroup("rempah");

		// Connect the CoinCollected signal of each coin to AddScore function 
		for (int i = 0; i < coins.Count; i++)
		{
			Coin n = (Coin)coins[i];
			n.CoinCollected += OnCoinCollected; // Langsung connect ke method
		}

		// Connect the GingerCollected signal of each jahe to AddScore function
		for (int i = 0; i < rempah.Count; i++)
		{
			// Cek apakah node adalah Jahe
			if (rempah[i] is Jahe jahe)
			{
				jahe.GingerCollected += OnJaheCollected; // Langsung connect ke method
			}
		}
		for (int i = 0; i < rempah.Count; i++)
		{
			// Cek apakah node adalah Jahe
			if (rempah[i] is Pala pala)
			{
				pala.GingerCollected += OnJaheCollected; // Langsung connect ke method
			}
		}
	}

	public void AddScore(int points = 1)
	{
		score += points;
		score_lbl.Text = "x " + score.ToString();
		
		// Debug print untuk memastikan AddScore dipanggil
		GD.Print($"Score updated: +{points}, Total: {score}");
	}

	// Method khusus untuk coin (1 poin)
	public void OnCoinCollected()
	{
		AddScore(1);
	}

	// Method khusus untuk jahe (sesuai nilai yang dikirim)
	public void OnJaheCollected(int points)
	{
		AddScore(points);
	}
}