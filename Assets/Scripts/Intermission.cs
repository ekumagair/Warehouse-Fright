using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Intermission : MonoBehaviour
{
    public string nextScene = "MainMenu";
    public float wait = 3;
    public Text scoreText;

    void Start()
    {
        Time.timeScale = 1.0f;

        if (scoreText != null)
        {
            scoreText.text += " " + Player.score.ToString();
        }
        if (wait >= 0)
        {
            StartCoroutine(Continue());
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            GoToScene();
        }
    }

    IEnumerator Continue()
    {
        yield return new WaitForSeconds(wait);

        GoToScene();
    }

    void GoToScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
