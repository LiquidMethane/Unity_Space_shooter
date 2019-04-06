using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_2 : Enemy
{
    [Header("Set in Inspector")]
    public float radius = 1f;
    public float rotationSpeed = 5f;

    float _angularFrequency = 0f; //omega
    float _random;
    bool _counterClockWise;

    public override void Move() //Enemy_2 moves in a spiral route either clockwise or counterclockwise
    {
        base.Move(); //moves downwards
        Vector3 tempPos = Vector3.zero;
        _angularFrequency += Time.deltaTime * rotationSpeed;
        tempPos.x = (_counterClockWise) ? radius * Mathf.Sin(_angularFrequency) : -radius * Mathf.Sin(_angularFrequency); //formula: position(t) = A * sin(wt), where w is angular frequency
        tempPos.y = radius * Mathf.Cos(_angularFrequency);

        Pos += tempPos;
    }

    void Start()
    {
        health = 3f;
        score = 10;
        _random = Random.value; //determines a random value for direction upon the creation of Enemy
        _counterClockWise = (_random > 0.5f) ? true : false;
        _angularFrequency = 0f;
    }


    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
