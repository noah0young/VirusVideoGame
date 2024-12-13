using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NearEndCollider : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.CompareTag("Player"))
		{
			TrashBoss.playerAtEnd();
		}
	}
}
