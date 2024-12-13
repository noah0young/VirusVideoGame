using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileControls : MonoBehaviour
{
    [Header("Horizontal")]
    public float maxDistanceFromHorizontal = 1f;
    public Transform horizontalCenter;
    [Header("Buttons")]
    public Transform jumpButtonPos;
    public Transform shootButtonPos;
    private TouchButton jumpButton;
    private TouchButton shootButton;
    // Start is called before the first frame update
    void Start()
    {
        jumpButton = new TouchButton(jumpButtonPos.position);
        shootButton = new TouchButton(shootButtonPos.position);
    }

    // Update is called once per frame
    void Update()
    {
        jumpButton.update();
        shootButton.update();
        for (int i = 0; i < Input.touches.Length; i++)
        {
            Debug.Log("Touch " + Input.touches[i] + " pos: " + Input.touches[i].position);
        }
    }

    public float horizontal()
    {
        float input = 0;
        foreach (Touch t in Input.touches)
        {
            if ((t.rawPosition  - ((Vector2)horizontalCenter.position)).SqrMagnitude() <= maxDistanceFromHorizontal)
            {
                float addedInput = (t.rawPosition - ((Vector2)horizontalCenter.position)).x / maxDistanceFromHorizontal;
                input += addedInput;
            }
        }
        return input;
    }

    public void pressJump()
    {
        jumpButton.startPressButton();
    }

    public void pressShoot()
    {
        shootButton.startPressButton();
    }

    public void releaseJump()
    {
        jumpButton.releaseButton();
    }

    public void releaseShoot()
    {
        shootButton.releaseButton();
    }

    public bool justPressedJump()
    {
        return jumpButton.justPressed();
    }

    public bool justPressedShoot()
    {
        return shootButton.justPressed();
    }

    public bool justReleasedJump()
    {
        return jumpButton.justReleased();
    }

    public bool justReleasedShoot()
    {
        return shootButton.justReleased();
    }
}

public class TouchButton
{
    private Vector2 buttonPos;
    private bool isPressed;
    private bool prevIsPressed;

    public TouchButton(Vector2 buttonPos)
    {
        this.buttonPos = buttonPos;
        isPressed = false;
        prevIsPressed = false;
    }

    public void update()
    {
        prevIsPressed = isPressed;
    }

    public void startPressButton()
    {
        isPressed = true;
    }

    public void releaseButton()
    {
        isPressed = false;
    }

    public bool pressed()
    {
        return isPressed;
    }

    public bool justPressed()
    {
        return isPressed && !prevIsPressed;
    }

    public bool justReleased()
    {
        return !isPressed && prevIsPressed;
    }

    public bool notPressed()
    {
        return !isPressed;
    }
}
