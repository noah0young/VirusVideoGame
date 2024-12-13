using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGotIt : MonoBehaviour
{
    public string nameInLevelManager;

    // Start is called before the first frame update
    void Start()
    {
        if (LevelManager.loadData && LevelManager.playerHasIt(nameInLevelManager))
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
