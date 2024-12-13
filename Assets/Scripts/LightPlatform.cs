using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPlatform : MonoBehaviour
{
    public SpriteRenderer mySprite;
    public Collider2D myCollider;
    public Sprite lightOffSprite;
    public Sprite lightOnSprite0;
    public Sprite lightOnSprite1;
    public bool isOn;
    public float animTime = .1f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(animate());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleLight()
    {
        myCollider.isTrigger = !myCollider.isTrigger;
        isOn = !isOn;

        if (isOn)
        {
            mySprite.sprite = lightOnSprite0;
        }
        else
        {
            mySprite.sprite = lightOffSprite;
        }
    }

    private IEnumerator animate()
    {
        while (true)
        {
            yield return new WaitForSeconds(animTime);
            if (isOn)
            {
                mySprite.sprite = lightOnSprite1;
            }
            yield return new WaitForSeconds(animTime);
            if (isOn)
            {
                mySprite.sprite = lightOnSprite0;
            }
        }
    }
}
