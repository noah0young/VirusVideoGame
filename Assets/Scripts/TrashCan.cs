using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCan : MonoBehaviour
{
    public float seconds = 5;
    public int numeratorProbOfHeart = 1;
    public int denominatorProbOfHeart = 4;
    public GameObject heartPickUp;
    public GameObject bossDestroyedObj;
    void Start()
    {
        StartCoroutine(destroyAfterTime());
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("EnemyShield"))
        {
            Destroy(gameObject);
        }
        else if (collision.transform.CompareTag("Enemy"))
        {
            checkHitInTrash();
            LevelManager.EnemyDestroyMsg();
            if (Random.Range(0, denominatorProbOfHeart) < numeratorProbOfHeart)
            {
                GameObject newHeart = Instantiate(heartPickUp, collision.gameObject.transform.parent);
                newHeart.transform.position = collision.gameObject.transform.position;
            }
            spawnEnemyPostDestroy(collision.gameObject);
            if (!LevelManager.trashIsLoaded())
            {
                Destroy(collision.gameObject);
            }
            Destroy(gameObject);
        }
        else if (collision.transform.CompareTag("Boss") || collision.transform.CompareTag("TrashBoss"))
        {
            checkHitInTrash();
            GameObject.Find("Player").GetComponent<Player>().Save();
            //LevelManager.LoadEpilogue();
            GameObject bossDest = Instantiate(bossDestroyedObj);
            bossDest.transform.position = collision.gameObject.transform.position;
            if (!LevelManager.trashIsLoaded())
            {
                Destroy(collision.gameObject);
            }
            Destroy(gameObject);
        }

        /*if (!collision.transform.CompareTag("Player"))
        {
            Destroy(gameObject);
        }*/
    }

    private IEnumerator destroyAfterTime()
    {
        yield return new WaitForSeconds(seconds);
        Destroy(gameObject);
    }

    // EFFECT: Spawns post destroy effect if it exists
    private void spawnEnemyPostDestroy(GameObject enemyObj)
    {
        BasicEnemy enemy = enemyObj.GetComponent<BasicEnemy>();
        if (enemy != null)
        {
            GameObject postDestory = Instantiate(enemy.postDestroyObj);
            postDestory.transform.position = enemyObj.transform.position;
            postDestory.transform.localScale = enemyObj.transform.localScale;
        }
    }

    private void checkHitInTrash()
    {
        if (LevelManager.trashIsLoaded())
        {
            TrashBoss.enemyHitInTrash();
        }
    }
}
