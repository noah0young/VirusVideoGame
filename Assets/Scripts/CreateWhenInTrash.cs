using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWhenInTrash : MonoBehaviour
{
    public GameObject loadedObj;
    // Start is called before the first frame update
    void Start()
    {
        if (LevelManager.trashIsLoaded() && loadedObj != null)
        {
            GameObject newObj = Instantiate(loadedObj);
            newObj.transform.position = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
