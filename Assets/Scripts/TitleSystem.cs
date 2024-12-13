using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TitleSystem : MonoBehaviour
{
	public Button continueButton;
	// Start is called before the first frame update
	void Start()
	{
		continueButton.interactable = PlayerPrefs.HasKey("hasDoubleJump");
	}

	// Update is called once per frame
	void Update()
    {
        
    }
}
