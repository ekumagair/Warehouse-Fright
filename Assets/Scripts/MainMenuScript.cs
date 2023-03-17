using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public GameObject selectArrow;
    public Text highScoreText;
    public AudioClip clipSelect;
    public AudioClip clipDelete;

    int option = 0;
    bool selecting = false;
    float delete = 0;
    SpriteRenderer arrowSr;
    AudioSource audioSource;

    void Start()
    {
        Time.timeScale = 1.0f;
        Screen.SetResolution(1024, 960, true);

        option = 0;
        delete = 0;
        selecting = false;
        arrowSr = selectArrow.GetComponent<SpriteRenderer>();
        arrowSr.enabled = true;
        audioSource = GetComponent<AudioSource>();

        Player.score = 0;
        Player.stage = 1;
        Player.stageDisplay = 1;
        Player.lives = 3;
        Player.scoreForExtraLife = 8000;
        Player.sceneTime = 0;
        GlobalScript.difficulty = 0;
        GlobalScript.gameIsInfinite = false;

        if (PlayerPrefs.HasKey("highScore_Normal"))
        {
            GlobalScript.highScoreNormal = PlayerPrefs.GetInt("highScore_Normal");
            GlobalScript.highScoreInfinite = PlayerPrefs.GetInt("highScore_Infinite");
        }
    }

    void Update()
    {
        if(option == 0)
        {
            selectArrow.transform.position = new Vector3(-3, -1.25f, 0);
            highScoreText.text = "HIGH SCORE: " + GlobalScript.highScoreNormal.ToString();
        }
        else if (option == 1)
        {
            selectArrow.transform.position = new Vector3(-3, -1.75f, 0);
            highScoreText.text = "HIGH SCORE: " + GlobalScript.highScoreInfinite.ToString();
        }

        if(option < 0)
        {
            option = 1;
        }
        if(option > 1)
        {
            option = 0;
        }

        if (selecting == false)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                option--;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftShift))
            {
                option++;
            }
            if(Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(Select());
            }
        }

        // Delete save
        if(Input.GetKey(KeyCode.Delete))
        {
            delete += Time.deltaTime;

            if(delete > 3)
            {
                DeleteSave();
            }
        }
        else
        {
            delete = 0;
        }

        // Quit game
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void DeleteSave()
    {
        delete = 0;
        PlayerPrefs.DeleteAll();
        GlobalScript.highScoreNormal = 0;
        GlobalScript.highScoreInfinite = 0;

        audioSource.PlayOneShot(clipDelete);
    }

    IEnumerator Select()
    {
        selecting = true;

        audioSource.PlayOneShot(clipSelect);

        for (int i = 0; i < 10; i++)
        {
            arrowSr.enabled = !arrowSr.enabled;
            yield return new WaitForSeconds(0.1f);
        }

        arrowSr.enabled = false;

        if (option == 0)
        {
            GlobalScript.gameIsInfinite = false;
        }
        else if (option == 1)
        {
            GlobalScript.gameIsInfinite = true;
        }

        SceneManager.LoadScene("Stage1");
    }
}
