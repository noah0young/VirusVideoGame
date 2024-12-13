using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    public bool doesNotMove = false;
    private static List<Camera> cameras = new List<Camera>();
    private static CameraEffects curCameraEffects;
    private Animator anim;
    public GameObject[] goneWhenErased;

    public bool isStartingCamera = false;
    // Start is called before the first frame update
    /*public static void init()
    {
        cameras = new List<Camera>();
    }*/
    
    void Start()
    {
        anim = GetComponent<Animator>();
        addCamera(GetComponent<Camera>());
        if (curCameraEffects == null && isStartingCamera)
        {
            curCameraEffects = this;
        }
        else if (this != curCameraEffects)
        {
            gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void cameraScreenShake()
    {
        anim.SetTrigger("ScreenShake");
    }

    public void cameraEraseTrash()
    {
        if (LevelManager.trashIsLoaded())
        {
            foreach (GameObject g in goneWhenErased)
            {
                g.SetActive(false);
            }
            anim.SetTrigger("EraseEverything");
        }
    }

    public static void eraseTrash()
    {
        curCameraEffects.cameraEraseTrash();
    }

    public static void screenShake()
    {
        curCameraEffects.cameraScreenShake();
    }

    public static void slowDownTime(float speed, float sec)
    {
        curCameraEffects.StartCoroutine(curCameraEffects.slowDownTimeRoutine(speed, sec));
    }

    public IEnumerator slowDownTimeRoutine(float speed, float sec)
    {
        float oldTimeScale = Time.timeScale;
        Time.timeScale = speed;
        yield return new WaitForSecondsRealtime(sec);
        /*if (Time.timeScale == speed)
        {
            Time.timeScale = oldTimeScale;
        }*/
        Time.timeScale = oldTimeScale;
    }

    public static void stopAndSlowDown(float stopSec, float speed, float sec)
    {
        curCameraEffects.StartCoroutine(curCameraEffects.stopAndSlowDownRoutine(stopSec, speed, sec));
    }

    public IEnumerator stopAndSlowDownRoutine(float stopSec, float speed, float sec)
    {
        float oldTimeScale = Time.timeScale;
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime(stopSec);
        /*if (Time.timeScale == speed)
        {
            Time.timeScale = oldTimeScale;
        }*/
        Time.timeScale = oldTimeScale;
        StartCoroutine(slowDownTimeRoutine(speed, sec));
    }

    public static void setCamera(Camera cam)
    {
        if (!cameras.Contains(cam))
        {
            addCamera(cam);
        }
        //if (cameras.Contains(cam))
        //{
            curCameraEffects = cam.GetComponent<CameraEffects>();
            foreach (Camera c in cameras)
            {
                c.gameObject.SetActive(false);
            }
            cam.gameObject.SetActive(true);
        // Toggles if the parallax background should move
        ParallaxBackrgound.setLockBackground(curCameraEffects.doesNotMove);
        //}
    }

    public static void addCamera(Camera cam)
    {
        if (!cameras.Contains(cam))
        {
            cameras.Add(cam);
        }
    }
    
    public static void removeCamera(Camera cam) {
        if (cameras.Contains(cam))
        {
            cameras.Remove(cam);
        }
    }

    private void OnDestroy()
    {
        removeCamera(this.GetComponent<Camera>());
    }

    public static Camera getCurCamera()
    {
        return curCameraEffects.GetComponent<Camera>();
    }
}
