using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragShootMovement : MonoBehaviour
{
    private Rigidbody2D playerRB;
    private Camera cam;
	public GameObject arrowGameObject;

    private Vector2 currentForce;
    private Vector3 startPoint;
    private Vector3 endPoint;

    public Vector2 minPower;
    public Vector2 maxPower;
    public float powerMultiplier;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        playerRB = GetComponent<Rigidbody2D>();
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
			SetTimeScale(0.05f);

			arrowGameObject.SetActive(true);
		}

		if (Input.GetMouseButtonUp(0))
		{
			endPoint = cam.ScreenToWorldPoint(Input.mousePosition);
			endPoint.z = 15;

			//currentForce = new Vector2(Mathf.Clamp(endPoint.x - transform.position.x, minPower.x, maxPower.x), Mathf.Clamp(endPoint.y - transform.position.y, minPower.y, maxPower.y));
			playerRB.velocity = transform.up * powerMultiplier;

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
}
