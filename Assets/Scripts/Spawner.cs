using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject obj;
    public float delay;
    public float delayBetween;
    public float directionX;
    public int objAmount;
    public int objLimit;

    void Start()
    {
        objAmount += GlobalScript.difficulty;

        if(objAmount > objLimit)
        {
            objAmount = objLimit;
        }
        if(objAmount < 1)
        {
            Destroy(gameObject);
        }

        StartCoroutine(Create());
    }

    IEnumerator Create()
    {
        yield return new WaitForSeconds(delay * 0.2f);

        for (int i = 0; i < objAmount; i++)
        {
            var inst = Instantiate(obj, transform.position, transform.rotation);

            if(inst.GetComponent<Projectile>() != null)
            {
                inst.GetComponent<Projectile>().speedX *= directionX;
            }

            yield return new WaitForSeconds(delayBetween);
        }

        yield return new WaitForSeconds(delay * 0.8f);

        StartCoroutine(Create());
    }
}
