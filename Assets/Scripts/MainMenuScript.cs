using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    [Header("Components")]
    public GameObject selectArrow;
    public Text highScoreText;
    public Text creditText;
    public Text versionText;

    [Header("Sounds")]
    public AudioClip clipSelect;
    public AudioClip clipDelete;

    private int _option = 0;
    private bool _selecting = false;
    private float _delete = 0;
    private SpriteRenderer _arrowSr;
    private AudioSource _audioSource;

    void Start()
    {
        Time.timeScale = 1.0f;

        _option = 0;
        _delete = 0;
        _selecting = false;
        _arrowSr = selectArrow.GetComponent<SpriteRenderer>();
        _arrowSr.enabled = true;
        _audioSource = GetComponent<AudioSource>();

        creditText.gameObject.SetActive(true);
        versionText.gameObject.SetActive(false);
        versionText.text = "VERSION " + Application.version;

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
        }

        if (PlayerPrefs.HasKey("highScore_Infinite"))
        {
            GlobalScript.highScoreInfinite = PlayerPrefs.GetInt("highScore_Infinite");
        }
    }

    void Update()
    {
        if (_option == 0)
        {
            selectArrow.transform.position = new Vector3(-3, -1.25f, 0);
            highScoreText.text = "HIGH SCORE: " + GlobalScript.highScoreNormal.ToString();
        }
        else if (_option == 1)
        {
            selectArrow.transform.position = new Vector3(-3, -1.75f, 0);
            highScoreText.text = "HIGH SCORE: " + GlobalScript.highScoreInfinite.ToString();
        }

        if (_option < 0)
        {
            _option = 1;
        }
        if (_option > 1)
        {
            _option = 0;
        }

        if (_selecting == false)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                _option--;
            }
            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftShift))
            {
                _option++;
            }
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StartCoroutine(Select());
            }
        }

        // Delete save.
        if (Input.GetKey(KeyCode.Delete))
        {
            _delete += Time.deltaTime;

            if (_delete > 3)
            {
                DeleteSave();
            }
        }
        else
        {
            _delete = 0;
        }

#if !UNITY_EDITOR && !UNITY_WEBGL && UNITY_STANDALONE
        // Quit game.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
#endif
    }

    private void DeleteSave()
    {
        _delete = 0;

        PlayerPrefs.DeleteAll();
        GlobalScript.highScoreNormal = 0;
        GlobalScript.highScoreInfinite = 0;
        PlayerPrefs.Save();

        _audioSource.PlayOneShot(clipDelete);
    }

    private IEnumerator Select()
    {
        _selecting = true;

        _audioSource.PlayOneShot(clipSelect);

        for (int i = 0; i < 10; i++)
        {
            _arrowSr.enabled = !_arrowSr.enabled;
            yield return new WaitForSeconds(0.1f);
        }

        _arrowSr.enabled = false;

        if (_option == 0)
        {
            GlobalScript.gameIsInfinite = false;
        }
        else if (_option == 1)
        {
            GlobalScript.gameIsInfinite = true;
        }

        SceneManager.LoadScene("Stage1");
    }

    public void ToggleVersion()
    {
        creditText.gameObject.SetActive(!creditText.gameObject.activeSelf);
        versionText.gameObject.SetActive(!versionText.gameObject.activeSelf);
    }
}