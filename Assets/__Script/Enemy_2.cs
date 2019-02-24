using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy
{
    [Header("Set in Inspector")]
    public float radius = 1f;
    public float rotationSpeed = 5f;

    float w = 0f;
    float random;
    bool counterClockWise;

    public override void Move()
    {
        base.Move();
        Vector3 tempPos = Vector3.zero;
        w += Time.deltaTime * rotationSpeed;
        tempPos.x = (counterClockWise) ? radius * Mathf.Sin(w) : -radius * Mathf.Sin(w);
        tempPos.y = radius * Mathf.Cos(w);

        Pos += tempPos;
        if (bndCheck != null && bndCheck.offDown)
        {

            Destroy(gameObject);

        }
    }

    void Start()
    {
        random = Random.value;
        counterClockWise = (random > 0.5f) ? true : false;
        w = 0f;
    }


    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
