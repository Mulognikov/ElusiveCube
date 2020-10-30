using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stars : MonoBehaviourExt, IListener
{
	private ParticleSystem particleSystem;
	private ParticleSystem.TrailModule trail;
	private ParticleSystem.VelocityOverLifetimeModule velocityOverLifetime;

	private Coroutine activeAction;

	private void Start()
	{
		particleSystem = GetComponent<ParticleSystem>();
		trail = particleSystem.trails;
		velocityOverLifetime = particleSystem.velocityOverLifetime;

		EventManager.Instance.AddListener(EventTypes.SuccessJump, this);
		EventManager.Instance.AddListener(EventTypes.PlayerReady, this);
		EventManager.Instance.AddListener(EventTypes.GameEnd, this);
	}

	public void OnEvent(EventTypes eventType, Component sender, object param = null)
	{
		switch (eventType)
		{
			case EventTypes.SuccessJump:
				OnSuccessJump();
				break;

			case EventTypes.PlayerReady:
				OnPlayerReady();
				break;

			case EventTypes.GameEnd:
				OnGameEnd();
				break;
		}
	}

	private void OnSuccessJump()
	{
		SwitchCoroutine(SpeedUpStars());
	}

	private void OnPlayerReady()
	{
		SwitchCoroutine(SpeedDownStars());
	}

	private void OnGameEnd()
	{
		SwitchCoroutine(SpeedUpStars());
	}

	private IEnumerator SpeedUpStars()
	{
		while (true)
		{
			velocityOverLifetime.speedModifier = new ParticleSystem.MinMaxCurve(Mathf.Lerp(velocityOverLifetime.speedModifier.constant, 7f, Time.deltaTime * 7f));
			trail.lifetime = new ParticleSystem.MinMaxCurve(Mathf.Lerp(trail.lifetime.constant, 0.015f, Time.deltaTime * 20f));

			yield return null;
		}
	}

	private IEnumerator SpeedDownStars()
	{
		while (true)
		{
			velocityOverLifetime.speedModifier = new ParticleSystem.MinMaxCurve(Mathf.Lerp(velocityOverLifetime.speedModifier.constant, 1f, Time.deltaTime * 2f));
			trail.lifetime = new ParticleSystem.MinMaxCurve(Mathf.Lerp(trail.lifetime.constant, 0f, Time.deltaTime * 1f));

			yield return null;
		}
	}
}
