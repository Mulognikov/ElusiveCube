using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Platform : MonoBehaviourExt, IListener
{
	public static Platform firstPlatform;
	private bool isTargetPlatform;

	private Rigidbody rigidbody;

	private float speed = 0.0275f;
	private float width = 2.85f;
	private float height = -1;

	private void Awake()
	{
		if (firstPlatform == null)
		{
			firstPlatform = this;
			isTargetPlatform = false;
		}
		else
		{
			isTargetPlatform = true;
		}
	}

	private void Start()
	{
		rigidbody = GetComponent<Rigidbody>();

		EventManager.Instance.AddListener(EventTypes.GameStart, this);
		EventManager.Instance.AddListener(EventTypes.GameEnd, this);
		EventManager.Instance.AddListener(EventTypes.GameContinue, this);
		EventManager.Instance.AddListener(EventTypes.SuccessJump, this);
		EventManager.Instance.AddListener(EventTypes.ScoreChange, this);
		EventManager.Instance.AddListener(EventTypes.PlayerReady, this);
		EventManager.Instance.AddListener(EventTypes.NextPlatformReady, this);
	}

	public void OnEvent(EventTypes eventType, Component sender, object param = null)
	{
		switch (eventType)
		{
			case EventTypes.GameEnd:
				OnGameEnd();
				break;

			case EventTypes.GameStart:
				OnGameStart();
				break;

			case EventTypes.GameContinue:
				OnGameStart();
				break;

			case EventTypes.SuccessJump:
				OnSuccessJump((float)param);
				break;

			case EventTypes.PlayerReady:
				OnPlayerReady();
				break;

			case EventTypes.NextPlatformReady:
				OnNextPlatfromReady((float)param);
				break;

			case EventTypes.ScoreChange:
				OnScoreChanges((int)param);
				break;

		}
	}

	private void OnGameStart()
	{
		if (!isTargetPlatform)
		{
			SwitchCoroutine(StartPlayerPlatform());
		}
	}

	private void OnGameEnd()
	{
		HidePlatform();
	}

	private IEnumerator StartPlayerPlatform()
	{
		yield return new WaitForSeconds(0.2f);

		transform.position = new Vector3(-5f, 1.65f, 0);
		transform.localScale = new Vector3(1, 0.25f, 1);

		Vector3 target = new Vector3(0, 1.65f, 0);

		while (true)
		{
			transform.position = Vector3.Lerp(transform.position, target, 7f * Time.deltaTime);
			yield return null;
		}
	}

	private IEnumerator SpawnTargetPlatform()
	{
		float direction = Mathf.Sign(Random.Range(-1, 1) + 0.5f);
		transform.position = new Vector3(5f * direction, height, 0);
		transform.localScale = new Vector3(width, 0.25f, 1);

		Vector3 target = new Vector3(0f * direction, height, 0);

		while (Vector3.Distance(transform.position, target) > 0.275f)
		{
			transform.position = Vector3.Lerp(transform.position, target, 5f * Time.deltaTime);
			yield return null;
		}

		EventManager.Instance.PostNotification(EventTypes.NextPlatformReady, this, direction);
	}

	private void OnNextPlatfromReady(float direction)
	{
		if (!isTargetPlatform)
			return;

		SwitchCoroutine(Coroutine());

		IEnumerator Coroutine()
		{
			speed = Mathf.Abs(speed) * -direction;
			float targetX = 1.5f * -direction;

			while (true)
			{
				if (Mathf.Abs(transform.position.x - targetX) < 0.05f)
				{
					speed = -speed;
					targetX = -targetX;
				}

				rigidbody.MovePosition(transform.position + Vector3.right * speed);
				yield return new WaitForFixedUpdate();
			}
		}
	}

	private void OnSuccessJump(float targetX)
	{
		if (!isTargetPlatform)
		{
			HidePlatform();
			isTargetPlatform = !isTargetPlatform;
			return;
		}

		isTargetPlatform = !isTargetPlatform;
		SwitchCoroutine(Coroutine());

		IEnumerator Coroutine()
		{
			Vector3 target = new Vector3(targetX, 1.65f, 0);
			Vector3 targetScale = new Vector3(1, 0.25f, 1f);

			while (true)
			{
				transform.position = Vector3.Lerp(transform.position, target, 7f * Time.deltaTime);
				transform.localScale = Vector3.Lerp(transform.localScale, targetScale, 7f * Time.deltaTime);
				yield return null;
			}
		}
	}

	private void OnPlayerReady()
	{
		if (isTargetPlatform)
		{
			SwitchCoroutine(SpawnTargetPlatform());
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		if (!isTargetPlatform)
		{
			return;
		}

		if (rigidbody.velocity == collision.rigidbody.velocity && Mathf.RoundToInt(collision.transform.rotation.eulerAngles.z) % 90 == 0)
		{
			float posX = collision.transform.position.x;

			if (Mathf.Abs(posX) > 1.5f)
			{
				posX = Mathf.Sign(posX) * 1.5f;
			}

			EventManager.Instance.PostNotification(EventTypes.SuccessJump, this, posX);
		}
	}

	private void OnScoreChanges(int score)
	{
		speed = 0.0275f + 0.000165f * score;
		width = 2.5f - 0.025f * score;
		height = -1f - 0.067f * score;

		if (score < 7f)
		{
			width += 0.3f;
		}

		Mathf.Clamp(width, 1.25f, 2.85f);
		Mathf.Clamp(height, -3, -1);
	}

	private void HidePlatform()
	{
		SwitchCoroutine(Coroutine());

		IEnumerator Coroutine()
		{
			Vector3 target = transform.position + Vector3.up * 10f;

			while (true)
			{
				transform.position = Vector3.Lerp(transform.position, target, 3f * Time.deltaTime);
				yield return null;
			}
		}
	}
}
