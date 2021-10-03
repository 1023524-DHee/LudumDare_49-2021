using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyer : MonoBehaviour
{
	private void Start()
	{
		StartCoroutine(StartDestruction());
	}

	private IEnumerator StartDestruction()
    {
		float startTime = Time.time;

		while (Time.time < startTime + 3f)
		{
			transform.localScale += new Vector3(10 * Time.deltaTime, 10 * Time.deltaTime, 0);
			yield return null;
		}
		Destroy(gameObject);
		yield break;
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy"))
		{
			Destroy(collision.gameObject);
		}
	}
}
