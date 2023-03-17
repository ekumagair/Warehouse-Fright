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
            GameObject.Find("Global").GetComponent<GlobalScript>().AddPoints(100 * Player.givePointsChain, true, transform, true);

            if (Player.usingLadder == false)
            {
                // givePointsChain: Increases exponentially everytime the player jumps over an obstacle. Multiplies the points added.
                // givePointsChainLinear: Doesn't multiply the points added. Increases by 1 for every obstacle hopped over before the player touches the ground.
                Player.givePointsChain *= 3;
                Player.givePointsChainLinear++;
            }
        }
        else if(collision.gameObject.tag == "Item")
        {
            audioSource.PlayOneShot(clipCollectItem);
            GameObject.Find("Global").GetComponent<GlobalScript>().AddPoints(1000, true, transform, false);
            Destroy(collision.gameObject);
        }
    }
}
