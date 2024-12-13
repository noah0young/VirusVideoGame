using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DesktopMusic : MonoBehaviour
{
	public bool isCalm = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.transform.CompareTag("Player") && isCalm)
		{
			LevelManager.StartCalmDesktopMusic();
		}
        else if (collision.transform.CompareTag("Player"))
		{
			LevelManager.StartAdvDesktopMusic();
		}
	}


}
