using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform playerGO;

	private float shakeDuration;
	private float shakeMagnitude = 1f;
	private float dampingSpeed = 2f;

    public Vector3 offset;
    public float smoothFactor;

	private void Start()
	{
		playerGO = GameObject.FindGameObjectWithTag("Player").transform;

		GameManager.current.onEnemyDeath += TriggerShake;
	}

	void Update()
	{
		Follow();

		if (shakeDuration > 0)
		{
			transform.position += Random.insideUnitSphere * shakeMagnitude;

			shakeDuration -= Time.deltaTime * dampingSpeed;
		}
	}

	private void Follow()
	{
		Vector3 targetPosition = playerGO.position + offset;
		Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.fixedDeltaTime);

		transform.position = smoothPosition;
	}

	private void TriggerShake()
	{
		shakeDuration = 0.25f;
	}
}
