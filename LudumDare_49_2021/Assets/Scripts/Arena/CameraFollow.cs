using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Transform playerGO;

    public Vector3 offset;
    public float smoothFactor;

	private void Start()
	{
		playerGO = GameObject.FindGameObjectWithTag("Player").transform;
	}

	private void FixedUpdate()
	{
		Follow();
	}

	private void Follow()
	{
		Vector3 targetPosition = playerGO.position + offset;
		Vector3 smoothPosition = Vector3.Lerp(transform.position, targetPosition, smoothFactor * Time.fixedDeltaTime);

		transform.position = smoothPosition;
	}
}
