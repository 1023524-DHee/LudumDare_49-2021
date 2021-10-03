using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pieces : MonoBehaviour
{
	private bool isMerged;
	private Rigidbody2D pieceRB;

	private void Start()
	{
		pieceRB = GetComponent<Rigidbody2D>();
	}

	private void Update()
	{
		if (isMerged && transform.parent != null)
		{
			pieceRB.velocity = transform.parent.position - transform.position;
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.CompareTag("Piece"))
		{
			transform.parent = collision.transform;
			isMerged = true;
		}
	}
}
