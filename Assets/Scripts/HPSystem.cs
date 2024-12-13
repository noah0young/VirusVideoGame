using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPSystem : MonoBehaviour
{
    private int maxHP;
    public Image[] hearts;
    public Sprite heartSprite0;
    //public Sprite heartSprite1;
    public Sprite emptyHeartSprite0;
    //public Sprite emptyHeartSprite1;
    //public float heartChangeTime;
    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(setHeartSprite());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setRemainingHearts(int hp)
    {
        for (int i = 0; i < maxHP; i++)
        {
            hearts[i].sprite = emptyHeartSprite0;
        }

        for (int i = 0; i < hp; i++)
        {
            hearts[i].sprite = heartSprite0;
        }
    }

    /*public IEnumerator setHeartSprite()
    {
        while(true)
        {
            foreach (Image heart in hearts)
            {
                heart.sprite = heartSprite0;
            }
            yield return new WaitForSeconds(heartChangeTime);
            foreach (Image heart in hearts)
            {
                heart.sprite = heartSprite1;
            }
            yield return new WaitForSeconds(heartChangeTime);
        }
    }*/

    public void setMaxHP(int mHP)
    {
        maxHP = mHP;
        for (int i = 0; i < maxHP; i++)
        {
            hearts[i].enabled = true;
        }
    }
}
