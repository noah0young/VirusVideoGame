using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
	public GameObject bossEnemy;
	public GameObject trashDoor;
	public float cutsceneTime = 5f;
    public float cutsceneTimeEnd = 1f;
    private Animator myAnim;
	public GameObject[] phase1Enemies;
	public GameObject[] phase2Enemies;
	public GameObject[] phase3Enemies;
    public bool inSecondCutscene = false;
    [Header("Dialogue")]
    public string[] bossDialogue1;
    public string[] bossDialogue2;
    public string[] bossDialogueDefeat;
    [Header("Cameras")]
    public Camera bossTalkingCamera;
    public Camera bossFightCamera;
    // Start is called before the first frame update
    void Start()
    {
        inSecondCutscene = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void StartCutscene(bool secondCutscene)
	{
        if (!secondCutscene)
        {
            myAnim = GetComponent<Animator>();
            StartCoroutine(Cutscene());
        } else
        {
            inSecondCutscene = true;
        }
	}

    private IEnumerator Cutscene()
    {
	    Camera origCamera = CameraEffects.getCurCamera();
        //GameObject playerCamera = GameObject.Find("Player/Main Camera");
        //playerCamera.SetActive(false);
        CameraEffects.setCamera(bossTalkingCamera);
        LevelManager.LoadBossCutsceneMusic();

		//myAnim.SetBool("CutScene", true);
        for (int i = 0; i < bossDialogue1.Length; i++) {
            StartCoroutine(LevelManager.PauseAndSpeak(bossDialogue1[i]));

            yield return new WaitUntil(() => !LevelManager.dialogueBoxText.transform.parent.gameObject.active);
        }
        myAnim.SetBool("CutSceneEnd", true);
        CameraEffects.setCamera(origCamera);
        yield return new WaitUntil(() => inSecondCutscene);
        
        for (int i = 0; i < bossDialogue2.Length; i++)
        {
            StartCoroutine(LevelManager.PauseAndSpeak(bossDialogue2[i]));
            
            yield return new WaitUntil(() => !LevelManager.dialogueBoxText.transform.parent.gameObject.active);
        }
        //playerCamera.SetActive(false);
        //bossFightCameraObj.SetActive(true);
        CameraEffects.setCamera(bossFightCamera);
        //yield return new WaitForSecondsRealtime(cutsceneTime);
        //myAnim.SetBool("CutSceneEnd", true);
        //yield return new WaitForSecondsRealtime(cutsceneTimeEnd);
        LevelManager.LoadBossMusic();
		myAnim.SetBool("BossPhase1", true);
		setAllActive(phase1Enemies);
		yield return new WaitUntil(() => areGameObjectsDestroyed(phase1Enemies));
		myAnim.SetBool("BossPhase2", true);
		setAllActive(phase2Enemies);
		yield return new WaitUntil(() => areGameObjectsDestroyed(phase2Enemies));
		myAnim.SetBool("BossPhase3", true);
		setAllActive(phase3Enemies);
		yield return new WaitUntil(() => areGameObjectsDestroyed(phase3Enemies));
		myAnim.SetBool("BossPhase4", true);
		// Post Boss Scene
		yield return new WaitUntil(() => bossEnemy == null);
		
		for (int i = 0; i < bossDialogueDefeat.Length; i++)
		{
			StartCoroutine(LevelManager.PauseAndSpeak(bossDialogueDefeat[i]));
            
			yield return new WaitUntil(() => !LevelManager.dialogueBoxText.transform.parent.gameObject.active);
		}

		Instantiate(trashDoor);
    }

    public bool areGameObjectsDestroyed(GameObject[] gameObjs)
	{
		bool allDestroyed = true;
        foreach(GameObject gameObj in gameObjs)
		{
			allDestroyed = allDestroyed && gameObj == null;
		}
		return allDestroyed;
	}

    public void setAllActive(GameObject[] gameObjs)
	{
        foreach (GameObject gameObj in gameObjs)
		{
			gameObj.SetActive(true);
		}
	}
}
