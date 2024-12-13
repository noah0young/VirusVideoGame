using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Door : MonoBehaviour
{
    public string nextScene = "";
    public int connectionNum = -1;
    //public bool saveAfterEntering = true;
    private TMP_Text aboveText;

    void Start()
    {
        aboveText = GetComponentInChildren<TMP_Text>();
        aboveText.text = "change directory to " + nextScene;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.setDoorPath(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            Player player = collision.GetComponent<Player>();
            player.resetDoorPath();
        }
    }
}
