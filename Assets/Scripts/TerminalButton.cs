using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerminalButton : MonoBehaviour
{
    private SpriteRenderer mySprite;
    public Sprite buttonUpSprite;
    public Sprite buttonDownSprite;
    public bool isDown;
    private LightPlatform[] lights;
    public MovingPlatform[] platforms;
    public bool turnsOffFirewall = false;
    public bool clearTrash = false;
    public bool togglesLights = true;
    public AudioSource pushSound;
    private static float eraseTime = 1f;
    // Start is called before the first frame update
    void Start()
    {
        mySprite = GetComponent<SpriteRenderer>();
        lights = new LightPlatform[0];
        if (togglesLights)
        {
            GameObject[] lightObjs = GameObject.FindGameObjectsWithTag("Lights");
            lights = new LightPlatform[lightObjs.Length];
            for (int i = 0; i < lights.Length; i++)
            {
                lights[i] = lightObjs[i].GetComponent<LightPlatform>();
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            pushSound.Play();
            isDown = true;
            mySprite.sprite = buttonDownSprite;
            toggleButton();
            if (turnsOffFirewall)
            {
                FireWall.turnOff();
                LevelManager.SaveFireWallOff();
            }

            if (clearTrash)
            {
                StartCoroutine(eraseTrash(collision.GetComponent<Player>()));
            }
        }
    }

    private IEnumerator eraseTrash(Player player)
    {
        player.freezePlayerPos();
        LevelManager.PlayErasingMusic();
        CameraEffects.eraseTrash();
        Time.timeScale = .5f;
        CameraEffects.stopAndSlowDown(.1f, .3f, .3f);
        yield return new WaitForSeconds(eraseTime);
        LevelManager.PlayErasedSound();
        yield return new WaitForSeconds(.1f);
        LevelManager.LoadEpilogue();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            pushSound.Play();
            isDown = false;
            mySprite.sprite = buttonUpSprite;
            //toggleButton();
        }
    }

    private void toggleButton()
    {
        foreach (LightPlatform light in lights)
        {
            light.toggleLight();
        }
        foreach (MovingPlatform plat in platforms)
        {
            plat.togglePos();
        }
    }
}
