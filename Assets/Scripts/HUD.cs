using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    public Text scoreText;
    public Text livesText;
    public Text stageText;
    public Text timeText;
    public Text timeUpText;
    public Text pauseText;
    public Text extraLifeText;
    public GameObject musicObject;

    AudioSource musicAudioSource;
    Player playerScript;

    void Start()
    {
        musicAudioSource = musicObject.GetComponent<AudioSource>();
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    void Update()
    {
        scoreText.text = Player.score.ToString();
        livesText.text = Player.lives.ToString();
        stageText.text = Player.stageDisplay.ToString();
        timeText.text = Player.time.ToString();
        extraLifeText.text = (Player.scoreForExtraLife / 1000).ToString() + "K=1-UP";
        
        //extraLifeText.text = playerScript.moveInputX.ToString();

        // Time up text
        if (Player.dead == true && Player.time <= 0)
        {
            timeUpText.enabled = true;
        }
        else
        {
            timeUpText.enabled = false;
        }

        // Stop music
        if((Player.dead == true || Player.winLevel == true) && musicObject != null)
        {
            Destroy(musicObject);
        }

        // Pause music
        if (musicAudioSource != null)
        {
            if (Time.timeScale != 0.0f)
            {
                musicAudioSource.UnPause();
            }
            else
            {
                musicAudioSource.Pause();
            }
        }

        // Show paused text
        if (Time.timeScale != 0.0f)
        {
            pauseText.enabled = false;
        }
        else
        {
            pauseText.enabled = true;
        }
    }
}
