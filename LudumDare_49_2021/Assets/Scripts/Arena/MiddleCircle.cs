using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiddleCircle : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Enemy"))
		{
			GameManager.current.EnemyDeath();
			GameManager.current.PlayerTakeDamage(0.5f);

			Destroy(collision.gameObject);
		}
	}
}
