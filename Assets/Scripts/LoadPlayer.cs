using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadPlayer : MonoBehaviour
{
    public GameObject playerObj;
    public GameObject introCamera;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartAnim());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator StartAnim()
    {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(1.6f);
        playerObj.SetActive(true);
        introCamera.SetActive(false);
        Time.timeScale = 1;
        Destroy(gameObject);
    }
}
