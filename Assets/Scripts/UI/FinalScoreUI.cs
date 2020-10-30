using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScoreUI : ScoreUI
{
	protected override void OnScoreChange(int score)
	{
		text.text = "Your score: " + score;
	}
}
