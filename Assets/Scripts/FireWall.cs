using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireWall : MonoBehaviour
{
    public static bool isOn = true;

    private void Start()
    {
        isOn = LevelManager.LoadFireWallOn();
    }

    private void Update()
    {
        if (!isOn)
        {
            gameObject.SetActive(false);
        }
    }

    public static void turnOff()
    {
        isOn = false;
        LevelManager.TurnedOffFirewallMsg();
    }
}
