using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTrigger : MonoBehaviour
{
    public AudioClip clipJumpOver;
    public AudioClip clipCollectItem;

    Player playerScript;
    AudioSource audioSource;

    void Start()
    {
        playerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Kill" || collision.gameObject.tag == "Ghost")
        {
            playerScript.Die();
            Destroy(gameObject);
        }
        else if(collision.gameObject.tag == "GivePoints" && Player.dead == false && Player.winLevel == false && playerScript.fallingTime < 1f)
        {
            audioSource.PlayOneShot(clipJumpOver);
            GameObject.Find("Global").GetComponent<GlobalScript>().AddPoints(100 * Player.givePointsChain, true, transform);

            if (Player.usingLadder == false)
            {
                Player.givePointsChain *= 3;
            }
        }
        else if(collision.gameObject.tag == "Item")
        {
            audioSource.PlayOneShot(clipCollectItem);
            GameObject.Find("Global").GetComponent<GlobalScript>().AddPoints(1000, true, transform);
            Destroy(collision.gameObject);
        }
    }
}
