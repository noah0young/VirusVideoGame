using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpCheck : MonoBehaviour
{
    private Player player;
    private int floorsOn = 0;

    void Start()
    {
        floorsOn = 0;
        player = transform.parent.GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == LayerMask.NameToLayer("Floor")) && !collision.isTrigger)
        {
            floorsOn++;
            if (floorsOn > 0)
            {
                //player.setInAir(false);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.layer == LayerMask.NameToLayer("Floor")) && !collision.isTrigger)
        {
            floorsOn--;
            if (floorsOn <= 0)
            {
                //player.setInAir(true);
            }
        }
    }

    public void resetFloorNum()
    {
        floorsOn = 0;
        //player.setInAir(true);
    }
}
