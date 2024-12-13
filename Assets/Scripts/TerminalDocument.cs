using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalDocument : MonoBehaviour
{
    public Collider2D myCollider;
    public SpriteRenderer mySprite;
    public float timeOffset = 0f;
    public float timeToChange = 2f;
    public Color walkableColor;
    public Color notWalkableColor;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(timedSwitchWalkable());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleWalkable()
    {
        bool ableToWalkOn = !myCollider.enabled;
        myCollider.enabled = ableToWalkOn;
        if (ableToWalkOn)
        {
            mySprite.color = walkableColor;
        }
        else
        {
            mySprite.color = notWalkableColor;
        }
    }

    private IEnumerator timedSwitchWalkable()
    {
        yield return new WaitForSeconds(timeOffset);
        while (true)
        {
            yield return new WaitForSeconds(timeToChange);
            toggleWalkable();
        }
    }
}
