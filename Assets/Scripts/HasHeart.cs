using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HasHeart : MonoBehaviour
{
    public int heartNum;
    // Start is called before the first frame update
    void Start()
    {
        if (Player.hasHearts[heartNum])
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
