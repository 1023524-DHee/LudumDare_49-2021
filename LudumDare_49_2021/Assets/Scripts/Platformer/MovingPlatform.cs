using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MovementDirection
{
    VERTICAL,
    HORIZONTAL
}

public class MovingPlatform : MonoBehaviour, IPausePlatform
{
    private float startTime;
    private float currentPlatformSpeed;

    public MovementDirection currentMovementDirection;

    public float platformSpeed;
    public float platformFrequency;

	private void Start()
	{
        currentPlatformSpeed = platformSpeed;
    }

	// Update is called once per frame
	void Update()
    {
        MovePlatform();
    }

    private void MovePlatform()
    {
        switch (currentMovementDirection)
        {
            case MovementDirection.VERTICAL:
                transform.position = new Vector3(transform.position.x, Mathf.Sin((Time.time - startTime) * platformFrequency) * currentPlatformSpeed);
                break;
            case MovementDirection.HORIZONTAL:
                transform.position = new Vector3(Mathf.Sin((Time.time - startTime) * platformFrequency) * currentPlatformSpeed, transform.position.y);
                break;
        }

    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.parent = transform;
            RestartPlatform();
        }
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
        if (collision.collider.CompareTag("Player"))
        {
            collision.collider.transform.parent = null;
        }
    }

	public void PausePlatform()
	{
        startTime = Time.time;
        currentPlatformSpeed = 0;
	}

	private void RestartPlatform()
	{
        currentPlatformSpeed = platformSpeed;
	}
}
