using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackrgound : MonoBehaviour
{
	private Transform playerTransform;
    private float playerOrigX;
    private Vector2 origPos;
    public float multiplier = 1;
    private static bool lockBackground = false;
    // Start is called before the first frame update
    void Start()
    {
        playerTransform = GameObject.FindGameObjectsWithTag("Player")[0].transform;
        playerOrigX = playerTransform.position.x;
        origPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!lockBackground)
        {
            moveWithPlayer();
        }
    }

    private void moveWithPlayer()
    {
        transform.position = new Vector2(origPos.x + (playerTransform.position.x - playerOrigX) * multiplier, origPos.y);
    }

    public static void setLockBackground(bool lockIt)
    {
        lockBackground = lockIt;
    }
}
