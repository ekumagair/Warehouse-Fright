using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public GameObject projectile;
    GameObject target;

    Animator animator;
    SpriteRenderer sr;
    Rigidbody2D rb;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(AttackLoop());
    }

    void Update()
    {
        if(target.transform.position.x > transform.position.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }

    IEnumerator AttackLoop()
    {
        yield return new WaitForSeconds(3f);

        if (Player.dead == false && Player.winLevel == false)
        {
            Attack();
            StartCoroutine(AttackLoop());
        }
    }

    void Attack()
    {
        var shot = Instantiate(projectile, transform.position, transform.rotation);

        if (sr.flipX == false)
        {
            shot.GetComponent<Projectile>().speedX *= -1;
        }

        animator.Play("GhostAttack");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Light" && Player.winLevel == false)
        {
            animator.Play("GhostDefeat");

            GameObject.Find("Global").GetComponent<GlobalScript>().AddPoints(2000 + (500 * GlobalScript.difficulty), true, transform);
            target.GetComponent<Player>().StartCoroutine(target.GetComponent<Player>().Win());
            StartCoroutine(GameObject.Find("Global").GetComponent<GlobalScript>().MajorDeath(gameObject, sr, rb, true));
            Player.winLevel = true;
        }
    }
}
