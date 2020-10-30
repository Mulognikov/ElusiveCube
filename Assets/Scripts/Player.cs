using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviourExt, IListener
{
	private bool ignoreTouch = true;
	private Rigidbody rigidbody;

	private void Start()
	{
		rigidbody = GetComponent<Rigidbody>();

		EventManager.Instance.AddListener(EventTypes.GameInit, this);
		EventManager.Instance.AddListener(EventTypes.GameStart, this);
		EventManager.Instance.AddListener(EventTypes.GameEnd, this);
		EventManager.Instance.AddListener(EventTypes.GameContinue, this);
		EventManager.Instance.AddListener(EventTypes.SuccessJump, this);
		EventManager.Instance.AddListener(EventTypes.PlayerReady, this);
		EventManager.Instance.AddListener(EventTypes.ScreenTouch, this);
	}

	public void OnEvent(EventTypes eventType, Component sender, object param = null)
	{
		switch (eventType)
		{
			case EventTypes.ScreenTouch:
				OnScreenToched((float)param);
				break;

			case EventTypes.GameInit:
				OnGameInit();
				break;

			case EventTypes.GameEnd:
				OnGameEnd();
				break;

			case EventTypes.GameContinue:
				OnGameStart();
				break;

			case EventTypes.GameStart:
				OnGameStart();
				break;

			case EventTypes.SuccessJump:
				OnSuccessJump((float)param);
				break;

			case EventTypes.PlayerReady:
				OnPlayerReady();
				break;
		}
	}

	private void OnGameInit()
	{
		SwitchCoroutine(MovePlayerMenuPos());
	}

	private void OnGameStart()
	{
		SwitchCoroutine(MovePlayerStartPos());
	}

	private void OnGameEnd()
	{
		SwitchCoroutine(MovePlayerMenuPos());
	}

	private void OnSuccessJump(float targetX)
	{
		SwitchCoroutine(MovePlayerNext(targetX));
	}

	private void OnPlayerReady()
	{
		SwitchCoroutine();

		rigidbody.isKinematic = false;
		ignoreTouch = false;
	}

	private void OnScreenToched(float direction)
	{
		if (ignoreTouch)
		{
			return;
		}

		ignoreTouch = true;
		SwitchCoroutine(MovePlayer(direction));
	}

	IEnumerator MovePlayerMenuPos()
	{
		rigidbody.isKinematic = true;
		while (true)
		{
			transform.Rotate(Vector3.up, 15f * Time.deltaTime);
			transform.position = Vector3.Lerp(transform.position, new Vector3(0, 0f, 0), 3f * Time.deltaTime);
			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 2, 3f * Time.deltaTime);
			yield return null;
		}
	}

	IEnumerator MovePlayerStartPos()
	{
		Vector3 target = new Vector3(0, 2.3f, 0);

		while (Vector3.Distance(transform.position, target) > 0.025f)
		{
			transform.position = Vector3.Lerp(transform.position, target, 7f * Time.deltaTime);
			transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one, 10f * Time.deltaTime);
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(Vector3.zero), 10f * Time.deltaTime);
			yield return null;
		}

		EventManager.Instance.PostNotification(EventTypes.PlayerReady, this);
	}

	IEnumerator MovePlayer(float direction)
	{
		Vector3 target = transform.position + new Vector3(1.25f, 0, 0) * direction;
		float time = 0;

		while (time <= 0.75f)
		{
			transform.position = Vector3.Lerp(transform.position, target, 3f * Time.deltaTime);
			yield return new WaitForFixedUpdate();
			time += Time.fixedDeltaTime;
		}
	}

	IEnumerator MovePlayerNext(float targetX)
	{
		Vector3 target = new Vector3(targetX, 2.275f, 0);
		Vector3 targetRotation = new Vector3(transform.rotation.eulerAngles.x, 0, transform.rotation.eulerAngles.z);
		rigidbody.isKinematic = true;

		while (Vector3.Distance(transform.position, target) >= 0.025f)
		{
			transform.position = Vector3.Lerp(transform.position, target, 7f * Time.deltaTime);
			transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(targetRotation), 10f * Time.deltaTime);
			yield return null;
		}

		EventManager.Instance.PostNotification(EventTypes.PlayerReady, this);
	}
}
