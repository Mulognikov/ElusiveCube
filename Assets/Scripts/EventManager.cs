using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventTypes
{
	ScreenTouch,
	GameInit,
	GameStart,
	GameEnd,
	GameContinue,
	SuccessJump,
	PlayerReady,
	NextPlatformReady,
	ScoreChange,
	TopScoreChange,
	AdReady,
	AdShow,
	GooglePlaySuccessAuth
}

public class EventManager : MonoBehaviour
{
	public static EventManager Instance
	{
		get { return instance; }
	}

	private static EventManager instance = null;
	private Dictionary<EventTypes, List<IListener>> listeners = new Dictionary<EventTypes, List<IListener>>();

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
		else
		{
			DestroyImmediate(this);
		}
	}

	public void AddListener(EventTypes eventType, IListener listener)
	{
		List<IListener> listenList;

		if (listeners.TryGetValue(eventType, out listenList))
		{
			listenList.Add(listener);
			return;
		}

		listenList = new List<IListener>();
		listenList.Add(listener);
		listeners.Add(eventType, listenList);
	}

	public void PostNotification(EventTypes eventType, Component sender, object param = null)
	{
		List<IListener> listenList;

		if (!listeners.TryGetValue(eventType, out listenList))
		{
			return;
		}

		foreach (IListener listener in listenList)
		{
			if (!listener.Equals(null))
			{
				listener.OnEvent(eventType, sender, param);
			}
		}
	}

	public void RemoveEvent(EventTypes eventType)
	{
		listeners.Remove(eventType);
	}

	public void RemoveRedundancies()
	{
		Dictionary<EventTypes, List<IListener>> tempListeners = new Dictionary<EventTypes, List<IListener>>();

		foreach (KeyValuePair<EventTypes, List<IListener>> item in listeners)
		{
			for (int i = item.Value.Count - 1; i >= 0; i--)
			{
				if (item.Value[i].Equals(null))
				{
					item.Value.RemoveAt(i);
				}
			}

			if (item.Value.Count > 0)
			{
				tempListeners.Add(item.Key, item.Value);
			}
		}

		listeners = tempListeners;
	}

	private void OnLevelWasLoaded(int level)
	{
		RemoveRedundancies();
	}
}
