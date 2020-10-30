using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class TopScore : MonoBehaviour, IListener
{
	private int topScore
	{
		get { return _topScore; }
		set
		{
			_topScore = value;
			PlayerPrefs.SetInt("TopScore", value);
			Social.ReportScore(value, GPGSIds.leaderboard_top_score, null);
			EventManager.Instance.PostNotification(EventTypes.TopScoreChange, this, value);
		}
	}

	private int _topScore;
	private bool firstUpdate = true;

	void Start()
    {
		EventManager.Instance.AddListener(EventTypes.ScoreChange, this);
		EventManager.Instance.AddListener(EventTypes.GooglePlaySuccessAuth, this);
	}

	private void Update()
	{
		if (firstUpdate)
		{
			firstUpdate = false;
			topScore = PlayerPrefs.GetInt("TopScore", 0);
		}
	}

	public void OnEvent(EventTypes eventType, Component sender, object param = null)
	{
		switch (eventType)
		{
			case EventTypes.ScoreChange:
				OnScoreChanged((int)param);
				break;

			case EventTypes.GooglePlaySuccessAuth:
				OnGooglePlaySuccessAuth();
				break;
		}
	}

	private void OnScoreChanged(int score)
	{
		if (score > topScore)
		{
			topScore = score;
		}
	}

	private void OnGooglePlaySuccessAuth()
	{
		PlayGamesPlatform.Instance.LoadScores(
			GPGSIds.leaderboard_top_score,
			LeaderboardStart.PlayerCentered,
			1,
			LeaderboardCollection.Public,
			LeaderboardTimeSpan.AllTime,
			(LeaderboardScoreData data) => {
				if (data.PlayerScore.value > topScore)
				{
					topScore = (int)data.PlayerScore.value;
				}
				else if (data.PlayerScore.value < topScore)
				{
					topScore = topScore;
				}
			});
	}
}
