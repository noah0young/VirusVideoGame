using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossCutscene : MonoBehaviour
{
    public GameObject door;
	public FinalBoss finalBoss;
    public bool isSecondCutscene = false;
    private bool hasPlayed = false;

    private void OnTriggerEnter2D(Collider2D collision)
	{
        if (collision.transform.CompareTag("Player") && !hasPlayed)
		{
            hasPlayed = true;
            finalBoss.StartCutscene(isSecondCutscene);
            if (door != null)
            {
                door.SetActive(true);
            }
        }
	}
}
