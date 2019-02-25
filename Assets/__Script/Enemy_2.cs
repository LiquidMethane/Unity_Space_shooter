using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy
{
    [Header("Set in Inspector")]
    public float radius = 1f;
    public float rotationSpeed = 5f;

    float _w = 0f; //omega
    float _random;
    bool _counterClockWise;

    public override void Move()
    {
        base.Move();
        Vector3 tempPos = Vector3.zero;
        _w += Time.deltaTime * rotationSpeed;
        tempPos.x = (_counterClockWise) ? radius * Mathf.Sin(_w) : -radius * Mathf.Sin(_w);
        tempPos.y = radius * Mathf.Cos(_w);

        Pos += tempPos;
        if (bndCheck != null && bndCheck.offDown)
        {

            Destroy(gameObject);

        }
    }

    void Start()
    {
        _random = Random.value;
        _counterClockWise = (_random > 0.5f) ? true : false;
        _w = 0f;
    }


    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
