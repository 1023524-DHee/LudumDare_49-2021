using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragShootMovement : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private Camera cam;
	
    private Vector3 endPoint;

    private bool buttonReleased;
    private float slowdownCounter;
    private float maxSlowdownTime = 0.7f;

    public GameObject arrowGameObject;

    public float powerMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        playerRB = GetComponent<Rigidbody2D>();

        GameManager.current.onPlayerDeath += OnPlayerDeath;

        slowdownCounter = maxSlowdownTime;
    }

    // Update is called once per frame
    void Update()
    {
        CheckInput();
    }

    private void CheckInput()
    {
        Vector3 currentPoint = cam.ScreenToWorldPoint(Input.mousePosition);
        currentPoint.z = 15;

        float angle = Mathf.Atan2(currentPoint.y - transform.position.y, currentPoint.x - transform.position.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle - 90));

        if (Input.GetMouseButtonDown(0))
		{
            slowdownCounter = maxSlowdownTime;
            SetTimeScale(0.1f);

            buttonReleased = false;
			arrowGameObject.SetActive(true);
		}

        if (Input.GetMouseButton(0))
        {
            slowdownCounter = Mathf.Clamp(slowdownCounter - Time.unscaledDeltaTime, 0, float.MaxValue);
        }

        if ((Input.GetMouseButtonUp(0) && !buttonReleased) || slowdownCounter <= 0f)
        {
            endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
            endPoint.z = 15;

            playerRB.velocity = transform.up * powerMultiplier;

            if (slowdownCounter <= 0f)
            {
                slowdownCounter = 999f;
            }

            buttonReleased = true;
            SetTimeScale(1f);
            arrowGameObject.SetActive(false);
        }
	}

	private void SetTimeScale(float scaleAmount)
    {
        Time.timeScale = scaleAmount;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.collider.CompareTag("Enemy"))
        {
            if (playerRB.velocity.magnitude > 10f)
            {
                ITakeDamage damageable = collision.collider.GetComponent<ITakeDamage>();

                damageable?.TakeDamage(10);
            }
        }
	}

    private void OnPlayerDeath()
    {
        CameraFollow.current.TriggerShake2(10f);
        Destroy(gameObject);
    }
}
