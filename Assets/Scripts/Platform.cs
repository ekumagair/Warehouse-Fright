using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public float speed;
    public Vector3 positionDestination;
    public int directionY = 0;

    private Vector3 _positionA;
    private Vector3 _positionB;
    private int _state = 0;

    void Start()
    {
        _positionA = transform.position;
        _positionB = transform.position + positionDestination;
    }

    void Update()
    {
        if (_state == 0)
        {
            if (transform.position.y < _positionB.y - 0.05f)
            {
                transform.Translate(0, speed * Time.deltaTime, 0);
                directionY = 1;
            }
            else if (transform.position.y > _positionB.y + 0.05f)
            {
                transform.Translate(0, -speed * Time.deltaTime, 0);
                directionY = -1;
            }
            else
            {
                _state = 1;
            }
        }
        else if (_state == 1)
        {
            if (transform.position.y < _positionA.y - 0.05f)
            {
                transform.Translate(0, speed * Time.deltaTime, 0);
                directionY = 1;
            }
            else if (transform.position.y > _positionA.y + 0.05f)
            {
                transform.Translate(0, -speed * Time.deltaTime, 0);
                directionY = -1;
            }
            else
            {
                _state = 0;
            }
        }
    }
}
