using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueSystem : MonoBehaviour
{
    public bool isEndOfGame = false;
    public bool startOnAwake = false;
    public TMP_Text linuxText;
    public string[] dialogue;
    public float[] pauseTime;
    private bool pauseDialogue = false;
    public static string friendTextStart = "<f>";
    public static string friendTextEnd = "</f>";
    //public string enterNameCode = "[Enter Name]";
    private static string friendTextStartCode = "<color=blue>";
    private static string friendTextEndCode = "</color>";
    /*[Header("Input System")]
    private string curInput = "";
    private string allowedChars = "abcdefghijklmnopqrstuvwxyz0123456789";*/
    public static string userName = "user";
    // Start is called before the first frame update
    void Start()
    {
        pauseDialogue = false;
        if (startOnAwake)
        {
            StartCoroutine(displayDialogue());
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator displayDialogue()
    {
        for (int i = 0; i < dialogue.Length; i++)
        {
            showText(i);
            yield return new WaitUntil(() => !pauseDialogue);
            yield return new WaitForSeconds(pauseTime[i]);
            linuxText.text = "";
            //removeFirstLine();
        }
        if (!isEndOfGame)
        {
            LoadNextScene();
        }
    }

    // EFFECT: Sets the current displayed text to the current dialogue of text
    private void showText(int dialgoueIndex)
    {
        /*if (dialogue[dialgoueIndex].Equals(enterNameCode))
        {
            input();
            userName = curInput;
            curInput = "";
        }*/
        linuxText.text += formateText(dialogue[dialgoueIndex]) + "\n";
    }

    // Purpose: Gets the text that each line starts with
    public static string linePrefix()
    {
        return userName + ":~$ ";
    }

    /*
     * Purpose: Returns the given string with a line prefix before each new line
     * EFFECT: Adds a line prefix before each new line
     */
    public static string formateText(string text)
    {
        text = replaceFriendMarks(text);
        text = linePrefix() + text;
        for (int i = 0; i < text.Length; i++)
        {
            if (text.Substring(i, 1).Equals("\n"))
            {
                text = text.Substring(0, i + 1) + linePrefix() + text.Substring(i + 1);
            }
        }
        return text;
    }

    public static string replaceFriendMarks(string text)
    {
        for (int i = 0; i <= text.Length - friendTextStart.Length && i <= text.Length - friendTextEnd.Length; i++)
        {
            if (i <= text.Length - friendTextStart.Length && text.Substring(i, friendTextStart.Length).Equals(friendTextStart))
            {
                text = text.Substring(0, i) + friendTextStartCode + text.Substring(i + friendTextStart.Length);
            }
            if (i <= text.Length - friendTextEnd.Length && text.Substring(i, friendTextEnd.Length).Equals(friendTextEnd))
            {
                text = text.Substring(0, i) + friendTextEndCode + text.Substring(i + friendTextEnd.Length);
            }
        }
        return text;
    }

    /*private IEnumerator input()
    {
        curInput = "";
        pauseDialogue = true;
        while (Input.GetKeyUp("enter"))
        {
            string keyPressed = Event.current.keyCode.ToString();
            if (allowedChars.Contains(keyPressed)) {
                curInput += keyPressed;
            }
            yield return new WaitForEndOfFrame();
        }
        pauseDialogue = false;
    }*/

    public void LoadNextScene()
    {
        LevelManager.LoadCurrentLevel();
    }

    public void removeFirstLine()
    {
        string str = linuxText.text;
        str.Substring(str.IndexOf("\n") + 1);
        linuxText.text = str;
    }
}
