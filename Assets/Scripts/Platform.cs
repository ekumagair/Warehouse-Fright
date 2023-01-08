using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float speed;
    public Vector3 positionDestination;
    Vector3 positionA;
    Vector3 positionB;
    int state = 0;
    public int directionY = 0;

    void Start()
    {
        positionA = transform.position;
        positionB = transform.position + positionDestination;
    }

    void Update()
    {
        if(state == 0)
        {
            if(transform.position.y < positionB.y - 0.05f)
            {
                transform.Translate(0, speed * Time.deltaTime, 0);
                directionY = 1;
            }
            else if (transform.position.y > positionB.y + 0.05f)
            {
                transform.Translate(0, -speed * Time.deltaTime, 0);
                directionY = -1;
            }
            else
            {
                state = 1;
            }
        }
        else if (state == 1)
        {
            if (transform.position.y < positionA.y - 0.05f)
            {
                transform.Translate(0, speed * Time.deltaTime, 0);
                directionY = 1;
            }
            else if (transform.position.y > positionA.y + 0.05f)
            {
                transform.Translate(0, -speed * Time.deltaTime, 0);
                directionY = -1;
            }
            else
            {
                state = 0;
            }
        }
    }
}
