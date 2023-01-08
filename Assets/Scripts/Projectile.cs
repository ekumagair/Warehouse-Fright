using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speedX = 0;
    public float speedY = 0;
    public bool destroyedByLight = false;
    public bool destroyedByWall = false;
    public int pointsOnDestroy = 0;
    public bool mirrorX = false;
    public GameObject createOndestroy;

    public LayerMask wallMask;
    RaycastHit2D rayHit;
    SpriteRenderer sr;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }


    void Update()
    {
        transform.Translate(speedX * Time.deltaTime, speedY * Time.deltaTime, 0);

        if(transform.position.x < -8 || transform.position.x > 8)
        {
            Destroy(gameObject);
        }
        if (transform.position.z < -8 || transform.position.z > 8)
        {
            Destroy(gameObject);
        }

        if(destroyedByWall)
        {
            if(speedX != 0)
            {
                rayHit = Physics2D.BoxCast(transform.position + (transform.right * Mathf.Clamp(speedX, -0.25f, 0.25f)), new Vector2(0.05f, 0.05f), 0, -transform.up, 0f, wallMask);
                if (rayHit.collider != null)
                {
                    Destroy(gameObject);
                }
            }
        }

        if(mirrorX)
        {
            if(speedX > 0)
            {
                sr.flipX = true;
            }
            else
            {
                sr.flipX = false;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Light" && destroyedByLight)
        {
            if(pointsOnDestroy != 0)
            {
                GameObject.Find("Global").GetComponent<GlobalScript>().AddPoints(pointsOnDestroy, true, transform);
            }
            if(createOndestroy != null)
            {
                Instantiate(createOndestroy, transform.position, transform.rotation);
            }

            Destroy(gameObject);
        }
    }
}
