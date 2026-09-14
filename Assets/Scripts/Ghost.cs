using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ghost : MonoBehaviour
{
    public GameObject projectile;

    private GameObject _target;
    private Animator _animator;
    private SpriteRenderer _sr;
    private Rigidbody2D _rb;

    void Start()
    {
        _target = GameObject.FindGameObjectWithTag("Player");
        _animator = GetComponent<Animator>();
        _sr = GetComponent<SpriteRenderer>();
        _rb = GetComponent<Rigidbody2D>();

        StartCoroutine(AttackLoop());
    }

    void Update()
    {
        if (_target.transform.position.x > transform.position.x)
        {
            _sr.flipX = true;
        }
        else
        {
            _sr.flipX = false;
        }
    }

    private IEnumerator AttackLoop()
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

        if (_sr.flipX == false)
        {
            shot.GetComponent<Projectile>().speedX *= -1;
        }

        _animator.Play("GhostAttack");
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Light" && Player.winLevel == false)
        {
            _animator.Play("GhostDefeat");

            GlobalScript.Instance.AddPoints(2000 + (500 * GlobalScript.difficulty), true, transform, false);
            _target.GetComponent<Player>().StartCoroutine(_target.GetComponent<Player>().Win());
            StartCoroutine(GlobalScript.Instance.MajorDeath(gameObject, _sr, _rb, true));
            Player.winLevel = true;
        }
    }
}
