using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    float _random;
    bool _touchBound, _touchLeftBound, _touchRightBound;

    public override void Move() //Enemy_1 moves in a diagonal line randomly to the left or to the right
    {
        base.Move(); //moves downward
        Vector3 tempPos = Vector3.zero;

        tempPos.x += (_random < 0.5f) ? speed * Time.deltaTime : -speed * Time.deltaTime; //moves leftward or rightward randomly
  
        Pos += tempPos;

    }
    
    protected override void Start()
    {
        base.Start();
        _random = Random.value; //determine the random value upon the creation of Enemy
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
