using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSec : MonoBehaviour
{
	public float time;
    // Start is called before the first frame update
    void Start()
    {
		StartCoroutine(destroyAfterTime());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator destroyAfterTime()
	{
		yield return new WaitForSeconds(time);
		Destroy(this.gameObject);
	}
}
