using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBossFollow : MonoBehaviour
{
    [Header("Go to Start")]
    public Transform followStartPos;
    public float timeUntilAtStart = 1f;
    private float timeUntilStartRemaining;
    private Vector2 speedToGetToStart;
    [Header("Follow")]
    private bool isFollowing = false;
    private Transform target;
    private Vector2 curSpeed;
    private Rigidbody2D myRigidbody;
    public Vector2 normalSpeed = new Vector2(5f, 3f);
    public Vector2 catchupSpeed = new Vector2(8f, 5f);
    public float startCatchupDistance = 5f;
    public float endCatchupDistance = 3f;
    [Header("Size Inc")]
    public float maxSize = 5f;
    public float timeUntilMaxSize = 5f;

    

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        curSpeed = normalSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowing)
        {
            follow();
            incSize();
        }
    }

    public void startFollow(Transform target)
    {
        
        this.target = target;
        
        StartCoroutine(goToStart());
    }

    private IEnumerator goToStart()
    {
        timeUntilStartRemaining = timeUntilAtStart;
        speedToGetToStart = (followStartPos.position - transform.position) * (1 / timeUntilAtStart);
        yield return new WaitUntil(() => moveUntilAtStart());
        isFollowing = true;
		// Allows the boss to hit you
		GetComponent<Collider2D>().isTrigger = false;
	}

    private bool moveUntilAtStart()
    {
        myRigidbody.velocity = speedToGetToStart;

        timeUntilStartRemaining -= Time.deltaTime;
        return timeUntilStartRemaining <= 0;
    }

    private void follow()
    {
        setCurSpeed();
        
        myRigidbody.velocity = (target.position - transform.position).normalized * curSpeed;
    }

    private void setCurSpeed()
    {
        Vector3 targetLoc = target.position;
        Vector3 vectorDistance = transform.position - targetLoc;
        float distance = Mathf.Pow(Mathf.Pow(vectorDistance.x, 2) + Mathf.Pow(vectorDistance.y, 2), .5f);
        if (distance > startCatchupDistance && curSpeed.x <= normalSpeed.x)
        {
            curSpeed = catchupSpeed;
        }
        else if (distance < endCatchupDistance && curSpeed.x >= catchupSpeed.x)
        {
            curSpeed = normalSpeed;
        }
    }

    private void incSize()
    {
        if (transform.localScale.x < maxSize)
        {
            Vector3 newScale = transform.localScale;
            newScale += Vector3.one * (1 / timeUntilMaxSize) * maxSize * Time.deltaTime;
            transform.localScale = newScale;
        }
        else
        {
            transform.localScale = Vector3.one * maxSize;
        }
    }
}
