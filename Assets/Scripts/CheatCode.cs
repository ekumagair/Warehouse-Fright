using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class CheatCode : MonoBehaviour
{
    [Header("Properties")]
    public bool once = true;

    [Header("Input")]
    public KeyCode[] buttons;
    public int currentButton;

    [Header("Cheat Effects")]
    public bool cheatExtraLives = false;
    public bool cheatContinue = false;
    public bool cheatDifficulty = false;
    public UnityEvent onCheatTyped;

    void Start()
    {
        currentButton = 0;
    }

    void OnGUI()
    {
        Event e = Event.current;

        if (e.isKey && Event.current.type == EventType.KeyUp)
        {
            if (buttons[currentButton] == e.keyCode)
            {
                // Check next key.
                currentButton++;

                // If typed every key.
                if (currentButton == buttons.Length)
                {
                    if (GlobalScript.debug == true)
                    {
                        Debug.Log("Finished code");
                    }

                    ExecuteEffect();
                }
            }
            else
            {
                currentButton = 0;
            }
        }
    }

    private void ExecuteEffect()
    {
        currentButton = 0;

        if (cheatExtraLives == true)
        {
            Player.lives = 30;
        }
        if (cheatContinue == true && GlobalScript.gameIsInfinite == false)
        {
            Player.score = 0;
            Player.lives = 3;
            Player.scoreForExtraLife = 8000;
            Player.sceneTime = 0;
            SceneManager.LoadScene("Stage" + Player.stage);
        }
        if (cheatDifficulty == true)
        {
            GlobalScript.difficulty = 9;
        }

        onCheatTyped?.Invoke();

        if (GetComponent<AudioSource>() != null)
        {
            GetComponent<AudioSource>().Play();
        }

        if (once == true)
        {
            cheatExtraLives = false;
            cheatContinue = false;
            cheatDifficulty = false;
            onCheatTyped = null;
        }
    }
}
