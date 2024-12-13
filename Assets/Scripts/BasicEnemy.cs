using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public LayerMask enemyWallCheckMask;
    private Collider2D myCollider;
    private Rigidbody2D myRigidbody;
    public bool movingRight = true;
    public float speed = 4;
    private static float nextToWallDistance = .53f;
    private static float endOfFloorDistance = .55f;
    public GameObject postDestroyObj;
    //private Collider2D endOfFloorCollider;
    //private Collider2D nextToWallCollider;
    // Start is called before the first frame update
    void Start()
    {
        myCollider = GetComponent<Collider2D>();
        myRigidbody = GetComponent<Rigidbody2D>();
        //endOfFloorCollider = GameObject.Find("ChangeDirectionFloorTrigger").GetComponent<Collider2D>();
        //nextToWallCollider = GameObject.Find("ChangeDirectionTrigger").GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        checkChangeDirection();
        move();
    }

    private void move()
    {
        float curSpeed = speed;
        if (!movingRight)
        {
            curSpeed = -curSpeed;
        }
        Vector2 velocity = myRigidbody.velocity;
        velocity.x = curSpeed;
        myRigidbody.velocity = velocity;
    }

    public void checkChangeDirection()
    {
        if (onGround())
        {
            bool wall = nextToWall();
            bool floor = endOfFloor();
            if (wall || floor)
            {
                movingRight = !movingRight;
            }
        }
    }

    public bool nextToWall()
    {
        /*return Physics2D.BoxCast(transform.position, myCollider.bounds.size, 0, Vector2.left, nextToWallDistance, enemyWallCheckMask) ||
            Physics2D.BoxCast(transform.position, myCollider.bounds.size, 0, Vector2.right, nextToWallDistance, enemyWallCheckMask);*/
        if (!movingRight && Physics2D.BoxCast(transform.position, myCollider.bounds.size, 0, Vector2.left, nextToWallDistance, enemyWallCheckMask))
        {
            return true;
        }
        else if (movingRight && Physics2D.BoxCast(transform.position, myCollider.bounds.size, 0, Vector2.right, nextToWallDistance, enemyWallCheckMask))
        {
            return true;
        }
        return false;
        /*return Physics2D.Raycast(transform.position, Vector2.left, nextToWallDistance) ||
            Physics2D.Raycast(transform.position, Vector2.right, nextToWallDistance);*/
    }

    public bool endOfFloor()
    {
        Vector3 leftCastPos = transform.position - new Vector3((myCollider.bounds.size.x / 2), 0, 0);
        Vector3 rightCastPos = transform.position + new Vector3((myCollider.bounds.size.x / 2), 0, 0);
        /*return !(Physics2D.Raycast(leftCastPos, Vector2.down, endOfFloorDistance) &&
            Physics2D.Raycast(rightCastPos, Vector2.down, endOfFloorDistance));*/
        if (!movingRight && !Physics2D.Raycast(leftCastPos, Vector2.down, endOfFloorDistance)) {
            return true;
        }
        else if (movingRight && !Physics2D.Raycast(rightCastPos, Vector2.down, endOfFloorDistance)) {
            return true;
        }
        return false;
    }

    public bool onGround()
    {
        /*Vector3 leftCastPos = transform.position - new Vector3((myCollider.bounds.size.x / 2), 0, 0);
        Vector3 rightCastPos = transform.position + new Vector3((myCollider.bounds.size.x / 2), 0, 0);
        return Physics2D.Raycast(leftCastPos, Vector2.down, endOfFloorDistance) ||
            Physics2D.Raycast(rightCastPos, Vector2.down, endOfFloorDistance);*/
        return true;
    }

    /*private void OnDestroy()
    {
        GameObject postDest = Instantiate(postDestroyObj);
        postDest.transform.position = transform.position;
    }*/
}
