using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashBoss : MonoBehaviour
{
    public string[] enemyWasHitDialogue;
    public string[] bossDialogue1;
    public string[] bossEndDialogue;
    private Animator myAnim;
    private static bool enemyWasHit = false;
    private static bool playerNearEnd = false;
    private static bool startDialogue = false;
    private TrashBossFollow follow;

    // Start is called before the first frame update
    void Start()
    {
        follow = GetComponentInChildren<TrashBossFollow>();
        enemyWasHit = false;
        playerNearEnd = false;
        startDialogue = false;
        myAnim = GetComponent<Animator>();
        StartCoroutine(Cutscene());
        StartCoroutine(IfAttack());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private IEnumerator Cutscene()
    {
        yield return new WaitUntil(() => startDialogue);
        //LevelManager.NoMusic();

        for (int i = 0; i < bossDialogue1.Length; i++) {
            StartCoroutine(LevelManager.PauseAndSpeak(bossDialogue1[i]));

            yield return new WaitUntil(() => !LevelManager.dialogueBoxText.transform.parent.gameObject.active);
        }

        myAnim.SetTrigger("BossFollow");
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        follow.startFollow(playerObj.transform);
        LevelManager.PlayBackgroundMusic();
        // End Boss Scene

        yield return new WaitUntil(() => playerNearEnd);
		
		for (int i = 0; i < bossEndDialogue.Length; i++)
		{
			StartCoroutine(LevelManager.PauseAndSpeak(bossEndDialogue[i]));
            
			yield return new WaitUntil(() => !LevelManager.dialogueBoxText.transform.parent.gameObject.active);
		}
    }

    private IEnumerator IfAttack()
    {
        yield return new WaitUntil(() => enemyWasHit);
        for (int i = 0; i < enemyWasHitDialogue.Length; i++)
        {
            StartCoroutine(LevelManager.PauseAndSpeak(enemyWasHitDialogue[i]));

            yield return new WaitUntil(() => !LevelManager.dialogueBoxText.transform.parent.gameObject.active);
        }
    }

    public static void enemyHitInTrash()
    {
        enemyWasHit = true;
    }

    public static void playerAtEnd()
    {
        playerNearEnd = true;
    }

    public static void startDialogueCollided()
    {
        startDialogue = true;
    }
}
