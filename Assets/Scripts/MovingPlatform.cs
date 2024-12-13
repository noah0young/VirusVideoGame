using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    private Rigidbody2D myRigidbody;
    public Transform firstPos;
    public Transform secondPos;
    public bool usesButton = false;
    private bool atSecondPosWithButton = false;
    private IEnumerator curMovement;
    [Header("Timer")]
    public float seconds = 3f;
    private float timePassed = 0f;
    [Header("Move Objects On This")]
    private Vector3 changeInPos;
    private ArrayList transformsOnThis;
    // A list of transforms that are on this moving platform

    // Start is called before the first frame update
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        transformsOnThis = new ArrayList();
        changeInPos = new Vector3(0, 0, 0);
        if (!usesButton)
        {
            curMovement = movePlatform();
            StartCoroutine(curMovement);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        updateTimer();
        MoveObjsOnThis();
    }

    private IEnumerator movePlatform()
    {
        while(true)
        {
            // Moves to second pos
            //float timeStarted = Time.realtimeSinceStartup;
            //float timePassed = 0;
            resetTimer();
            Vector2 origPos = transform.position;
            while (!timerHasPassed())//timePassed / seconds < 1)
            {
                //timePassed = Time.realtimeSinceStartup - timeStarted;
                goTowards(origPos, secondPos.position);//, timePassed / seconds);
                //timePassed += Time.deltaTime / seconds;
                yield return new WaitForFixedUpdate();//new WaitForEndOfFrame();
            }
            //transform.position = secondPos.position;
            // Moves back to first pos
            origPos = secondPos.position;
            //timeStarted = Time.realtimeSinceStartup;
            //timePassed = 0;
            resetTimer();
            while (!timerHasPassed())//timePassed / seconds < 1)
            {
                //timePassed = Time.realtimeSinceStartup - timeStarted;
                goTowards(origPos, firstPos.position);//, timePassed / seconds);
                //timePassed += Time.deltaTime / seconds;
                yield return new WaitForFixedUpdate();//new WaitForEndOfFrame();
            }
            //transform.position = firstPos.position;
        }
    }

    private bool timerHasPassed()
    {
        return timePassed >= seconds;
    }

    private void updateTimer()
    {
        timePassed += Time.deltaTime;
        if (timerHasPassed())
        {
            timePassed = seconds;
        }
    }

    private void resetTimer()
    {
        timePassed = 0;
    }

    private void goTowards(Vector2 origPos, Vector2 loc)//, float timePassed)
    {
        /*Vector3 prevPos = transform.position;
        transform.position = Vector3.Lerp(origPos, loc, timePassed);
        changeInPos = transform.position - prevPos;*/
        myRigidbody.velocity = (loc - origPos) * (1 / seconds);
    }

    public void togglePos()
    {
        stopPrevMovement();
        if (!atSecondPosWithButton)
        {
            curMovement = moveToLoc(secondPos.position);
            StartCoroutine(curMovement);
            atSecondPosWithButton = true;
        }
        else
        {
            curMovement = moveToLoc(firstPos.position);
            StartCoroutine(curMovement);
            atSecondPosWithButton = false;
        }
    }

    private IEnumerator moveToLoc(Vector2 newLoc)
    {
        // Moves to second pos
        //float timeStarted = Time.realtimeSinceStartup;
        //float timePassed = 0;
        resetTimer();
        Vector2 origPos = transform.position;
        while (!timerHasPassed())//timePassed / seconds < 1)
        {
            //timePassed = Time.realtimeSinceStartup - timeStarted;
            goTowards(origPos, newLoc);//, timePassed / seconds);
            yield return new WaitForFixedUpdate();//new WaitForEndOfFrame();
        }
        myRigidbody.velocity = new Vector2(0, 0);
        transform.position = newLoc;
    }

    private void stopPrevMovement()
    {
        if (curMovement != null)
        {
            StopCoroutine(curMovement);
        }
    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        transformsOnThis.Add(collision.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        transformsOnThis.Remove(collision.transform);
    }*/

    private void MoveObjsOnThis()
    {
        foreach (object objOnThis in transformsOnThis)
        {
            Transform transformOnThis = (Transform)objOnThis;
            transformOnThis.position += changeInPos;
        }
    }
}
