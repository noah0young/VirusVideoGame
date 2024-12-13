using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFlashing : MonoBehaviour
{
    public Sprite[] sprites;
    public float waitTime = .1f;
    private Image mySprite;
    // Start is called before the first frame update
    void Start()
    {
        mySprite = GetComponent<Image>();
        StartCoroutine(switchSprite());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator switchSprite()
    {
        while(true)
        {
            for (int i = 0; i < sprites.Length; i++)
            {
                mySprite.sprite = sprites[i];
                yield return new WaitForSeconds(waitTime);
            }
        }
    }
}
