using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    float _random;
    bool _touchBound, _touchLeftBound, _touchRightBound;
    public override void Move()
    {
        base.Move();
        Vector3 tempPos = Vector3.zero;

        if (!_touchBound)
            tempPos.x += (_random < 0.5f) ? speed * Time.deltaTime : -speed * Time.deltaTime;
        else
        {
            if (_touchLeftBound)

                tempPos.x += speed * Time.deltaTime;
            if (_touchRightBound)

                tempPos.x -= speed * Time.deltaTime;
        }


        Pos += tempPos;

    }




    void Start()
    {
        _random = Random.value;


    }

    // Update is called once per frame
    void Update()
    {
        if (bndCheck.offLeft || bndCheck.offRight)
            _touchBound = true;
        if (bndCheck.offLeft)
        {
            _touchLeftBound = true;
            _touchRightBound = false;
        }
        else if (bndCheck.offRight)
        {
            _touchLeftBound = false;
            _touchRightBound = true;
        }
        Move();
    }
}
