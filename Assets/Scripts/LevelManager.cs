using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class LevelManager : MonoBehaviour
{
    //public static SaveData data;
    public bool isTitle = false;
    private static string curScene;
    private static AudioSource backgroundAudio;
    public static AudioClip rootMusic;
    public static AudioClip settingsMusic;
    public static AudioClip advDesktopMusic;
    public static AudioClip calmDesktopMusic;
    public static AudioClip bossMusic;
    public static AudioClip bossTalkingMusic;
    public static AudioClip trashMusic;
    public static AudioClip erasingMusic;
    public static AudioClip erasingMusicEnd;
    [Header("Terminal Canvas")]
    public static TMP_Text terminalText;
    public static TMP_Text dialogueBoxText;
    public static string terminalTextStart = "User:~$ ";
    public static string enemyDestroyedText = terminalTextStart + "Enemy was Deleted\n";
    public static string doorMsg = terminalTextStart + "Entered door\n";
    public static float timeUntilLineRemoved = 3f;

    public static bool loadData = false;
    // Start is called before the first frame update
    void Start()
    {
        if (!isTitle)// && curScene == null)
        {
            curScene = currentScene();
            backgroundAudio = GameObject.Find("BackgroundMusic").GetComponent<AudioSource>();
            rootMusic = (AudioClip)Resources.Load("Music/RootMusic");
            settingsMusic = (AudioClip)Resources.Load("Music/SettingsMusic");
            advDesktopMusic = (AudioClip)Resources.Load("Music/AdvDesktopMusic");
            calmDesktopMusic = (AudioClip)Resources.Load("Music/CalmDesktopMusic");
            bossTalkingMusic = (AudioClip)Resources.Load("Music/BossTalking");
            bossMusic = (AudioClip)Resources.Load("Music/TerminalBossMusic");
            trashMusic = (AudioClip)Resources.Load("Music/VirusTrashEscape");
            erasingMusic = (AudioClip)Resources.Load("Music/VirusErasing");
            erasingMusic = (AudioClip)Resources.Load("Music/VirusEraseEnd");
            SwitchBackgroundMusicToScene(curScene);
            terminalText = GameObject.Find("TerminalCanvas").GetComponentInChildren<TMP_Text>();
            dialogueBoxText = GameObject.Find("TerminalCanvas/DialogueBox").GetComponentInChildren<TMP_Text>();
            dialogueBoxText.transform.parent.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void SwitchCurScene(string newScene)
    {
        //CameraEffects.init();
        if (!curScene.Equals(newScene))
        {
            SwitchBackgroundMusicToScene(newScene);
        }
        SceneManager.UnloadSceneAsync(curScene);
        curScene = newScene;
        SceneManager.LoadScene(curScene, LoadSceneMode.Additive);
    }

    public static string currentScene()
    {
        Scene[] allOpenScenes = SceneManager.GetAllScenes();
        string currentScene;
        if (allOpenScenes[0].name.Equals("Player"))
        {
            currentScene = allOpenScenes[1].name;
        }
        else
        {
            currentScene = allOpenScenes[0].name;
        }
        return currentScene;
    }

    public static bool hasLoadedScene()
    {
        return currentScene().Equals(curScene);
    }

    public static void SwitchBackgroundMusicToScene(string newScene)
    {
        if (newScene.Equals("Root")) {
            backgroundAudio.clip = rootMusic;
        }
        else if (newScene.Equals("Settings"))
        {
            backgroundAudio.clip = settingsMusic;
        }
        else if (newScene.Equals("Trash"))
        {
            backgroundAudio.clip = trashMusic;
        }
        else if (newScene.Equals("Desktop"))
        {
            //backgroundAudio.clip = advDesktopMusic;
            backgroundAudio.Stop();
        }
        else if (newScene.Equals("Boss"))
        {
            backgroundAudio.clip = bossMusic;
        }

        if (!newScene.Equals("Trash"))
        {
            backgroundAudio.Play();
        }
        else
        {
            backgroundAudio.Stop();
        }
    }

    public static void StartAdvDesktopMusic()
    {
        if (backgroundAudio.clip != advDesktopMusic)
        {
            backgroundAudio.clip = advDesktopMusic;
            backgroundAudio.Play();
        }
    }

    public static void PlayBackgroundMusic()
    {
        backgroundAudio.Play();
    }

    public static void StartCalmDesktopMusic()
    {
        if (backgroundAudio.clip != calmDesktopMusic)
        {
            backgroundAudio.clip = calmDesktopMusic;
            backgroundAudio.Play();
        }
    }

    public static void PlayErasingMusic()
    {
        backgroundAudio.clip = erasingMusic;
        backgroundAudio.Play();
    }

    public static void PlayErasedSound()
    {
        backgroundAudio.clip = erasingMusicEnd;
        backgroundAudio.loop = false;
        backgroundAudio.Play();
    }

    public static void EnemyDestroyMsg()
    {
        terminalText.text += enemyDestroyedText;
        terminalText.StartCoroutine(RemoveTopLine());
    }

    public static void PrintCD()
    {
        terminalText.text += terminalTextStart + "cd " + curScene + "\n";
        terminalText.StartCoroutine(RemoveTopLine());
    }

    public static void PrintWelcome()
    {
        terminalText.text += terminalTextStart + "Welcome to your " + curScene + "!\n";
        terminalText.StartCoroutine(RemoveTopLine());
    }

    public static void EnteredDoorMsg()
    {
        terminalText.text += doorMsg;
        terminalText.StartCoroutine(RemoveTopLine());
    }

    public static void TurnedOffFirewallMsg()
    {
        terminalText.text += terminalTextStart + "Firewall has been deactivated\n";
        terminalText.StartCoroutine(RemoveTopLine());
    }

    public static void GotItem(string itemName)
    {
        terminalText.text += terminalTextStart + "Got " + itemName + "!\n";
        terminalText.StartCoroutine(RemoveTopLine());
    }

    private static IEnumerator RemoveTopLine()
    {
        yield return new WaitForSeconds(timeUntilLineRemoved);
        string curText = terminalText.text;
        terminalText.text = curText.Substring(curText.IndexOf("\n") + 1);
    }

    public static void LoadCurrentLevel()
    {
        SceneManager.LoadScene("Player");
        SceneManager.LoadScene("Root", LoadSceneMode.Additive);
    }

    public static void Save(Vector3 playerPos, bool hasPurpleEncryption, bool hasGreenEncryption, bool hasDoubleJump, /*int maxHP*/ bool[] hasHearts, int curHP, Player.EncState encState)
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetFloat("playerPos.x", playerPos.x);
        PlayerPrefs.SetFloat("playerPos.y", playerPos.y);
        PlayerPrefs.SetFloat("playerPos.z", playerPos.z);
        PlayerPrefs.SetInt("hasPurpleEncryption", hasPurpleEncryption ? 1 : 0);
        PlayerPrefs.SetInt("hasGreenEncryption", hasGreenEncryption ? 1 : 0);
        PlayerPrefs.SetInt("hasDoubleJump", hasDoubleJump ? 1 : 0);
        //PlayerPrefs.SetInt("maxHP", maxHP);
        for (int i = 0; i < hasHearts.Length; i++)
		{
			PlayerPrefs.SetInt("hasHeart" + i, hasHearts[i] ? 1 : 0);
		}
        PlayerPrefs.SetInt("curHP", curHP);
        PlayerPrefs.SetString("CurScene", curScene);
        PlayerPrefs.SetInt("EncState", encState == Player.EncState.NONE ? 0 : (encState == Player.EncState.PURPLE ? 1 : 2));
        PlayerPrefs.SetInt("FireWallOn", FireWall.isOn ? 1 : 0);
        PlayerPrefs.Save();
        //new SaveData(playerPos, hasPurpleEncryption, hasGreenEncryption, hasDoubleJump, hasHearts, curHP, encState).Save();
    }

    /*public static void Load()
    {
        data = SaveData.Load();
    }*/

    public static void SaveFireWallOff()
    {
        PlayerPrefs.SetInt("FireWallOn", 0);
    }

    public static bool LoadFireWallOn()
    {
        if (PlayerPrefs.HasKey("FireWallOn"))
        {
            return PlayerPrefs.GetInt("FireWallOn") == 1;
        }
        return true;
    }

    public static Player.EncState LoadEncState()
    {
        int encStateInt = PlayerPrefs.GetInt("EncState");
        if (encStateInt == 0)
        {
            return Player.EncState.NONE;
        }
        else if (encStateInt == 1)
        {
            return Player.EncState.PURPLE;
        }
        else
        {
            return Player.EncState.GREEN;
        }
    }

    public static Vector3 LoadPlayerPos()
    {
        return new Vector3(PlayerPrefs.GetFloat("playerPos.x"), PlayerPrefs.GetFloat("playerPos.y"), PlayerPrefs.GetFloat("playerPos.z"));
    }

    public static bool LoadHasPurpleEnc()
    {
        return PlayerPrefs.GetInt("hasPurpleEncryption") == 1;
    }

    public static bool LoadHasGreenEnc()
    {
        return PlayerPrefs.GetInt("hasGreenEncryption") == 1;
    }

    public static bool LoadHasDoubleJump()
    {
        return PlayerPrefs.GetInt("hasDoubleJump") == 1;
    }

    public static int LoadMaxHP()
    {
		int additionalHP = 0;
		for (int i = 0; i < 4; i++)
		{
			additionalHP += PlayerPrefs.GetInt("hasHeart" + i) == 1 ? 1 : 0;
		}
		return additionalHP + 2;
	}

    public static bool[] LoadHasHearts()
    {
        bool[] hasHearts = new bool[4];
        for (int i = 0; i < 4; i++)
        {
            hasHearts[i] = PlayerPrefs.GetInt("hasHeart" + i) == 1;
        }
        return hasHearts;
    }

    public static int LoadCurHP()
    {
        return PlayerPrefs.GetInt("curHP");
    }

    public static string LoadLastScene()
    {
        return PlayerPrefs.GetString("CurScene");
    }

    public static void LoadTitle()
    {
        SceneManager.LoadScene("Title");
    }

    public void LoadNewGame()
    {
        loadData = false;
        SceneManager.LoadScene("OpenningScene");
    }

    public void EraseSaveData()
    {
        PlayerPrefs.DeleteAll();
    }

    public void LoadContinue()
    {
        loadData = true;
        //CameraEffects.init();
        SceneManager.LoadScene("Player");
        SceneManager.LoadScene(LoadLastScene(), LoadSceneMode.Additive);
        //GameObject.Find("Player").GetComponent<Player>().LoadData(LoadPlayerPos(), LoadHasPurpleEnc(), LoadHasGreenEnc(), LoadHasDoubleJump(), LoadMaxHP(), LoadCurHP());
    }

    public static void LoadGameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    public static void LoadEpilogue()
    {
        SceneManager.LoadScene("EndScene");
    }

    public static void LoadBossCutsceneMusic()
    {
        backgroundAudio.clip = bossTalkingMusic;
        backgroundAudio.Play();
    }

    public static void LoadBossMusic()
    {
        backgroundAudio.clip = bossMusic;
        backgroundAudio.Play();
    }

    public static IEnumerator PauseAndSpeak(string dialogue)
    {
        string formattedText = DialogueSystem.formateText(dialogue);
        dialogueBoxText.transform.parent.gameObject.SetActive(true);
        dialogueBoxText.text = formattedText;
        float prevTimeScale = Time.timeScale;
        Time.timeScale = 0;

        yield return new WaitUntil(() => Input.GetKeyDown("space"));
        yield return new WaitUntil(() => Input.GetKeyUp("space"));
        //yield return new WaitUntil(() => Input.anyKeyDown);
        //yield return new WaitForEndOfFrame();
        dialogueBoxText.transform.parent.gameObject.SetActive(false);
        if (Time.timeScale == 0)
        {
            Time.timeScale = prevTimeScale;
        }
    }

    public static bool playerHasIt(string item)
    {
        if (item.Equals("Purple Encryption Key"))
        {
            return LoadHasPurpleEnc();
        }
        else if (item.Equals("Green Encryption Key"))
        {
            return LoadHasGreenEnc();
        }
        else if (item.Equals("Double Jump Sphere"))
        {
            return LoadHasDoubleJump();
        }
        return false;
    }

    public static bool trashIsLoaded()
    {
        return currentScene().Equals("Trash");
    }
}

/*public class SaveData
{
    private Vector3 playerPos;
    private bool hasPurple;
    private bool hasGreen;
    private bool hasDJump;
    private bool[] hasHearts;
    private int curHP;
    private Player.EncState encState;

    public SaveData (Vector3 playerPos, bool hasPurpleEncryption, bool hasGreenEncryption, bool hasDoubleJump, bool[] hasHearts, int curHP, Player.EncState encState)
    {
        this.playerPos = playerPos;
        hasPurple = hasPurpleEncryption;
        hasGreen = hasGreenEncryption;
        this.hasDJump = hasDoubleJump;
        this.hasHearts = hasHearts;
        this.curHP = curHP;
        this.encState = encState;
    }

    public void Save()
    {
        string jsonFileText = JsonUtility.ToJson(this);
        File.WriteAllText(Application.dataPath + "/saveData.txt", jsonFileText);
    }

    /*public void Load()
    {
        SaveData loadedData = JsonUtility.FromJson<SaveData>(Application.dataPath + "/saveData.txt");
        this.playerPos = loadedData.playerPos;
        this.hasPurple = loadedData.hasPurple;
        this.hasGreen = loadedData.hasGreen;
        this.hasDJump = loadedData.hasDJump;
        this.hasHearts = loadedData.hasHearts;
        this.curHP = loadedData.curHP;
        this.encState = loadedData.encState;
    }*

    public static SaveData Load()
    {
        SaveData loadedData = JsonUtility.FromJson<SaveData>(Application.dataPath + "/saveData.txt");
        return loadedData;
    }
}*/