using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_1 : Enemy
{
    float random;
    bool touchBound, touchLeftBound, touchRightBound;
    public override void Move()
    {
        base.Move();
        Vector3 tempPos = Vector3.zero;

        if (!touchBound)
            tempPos.x += (random < 0.5f) ? speed * Time.deltaTime : -speed * Time.deltaTime;
        else
        {
            if (touchLeftBound)

                tempPos.x += speed * Time.deltaTime;
            if (touchRightBound)

                tempPos.x -= speed * Time.deltaTime;
        }




        Pos += tempPos;


        if (bndCheck != null && bndCheck.offDown)
        {
            Destroy(gameObject);
        }
    }




    void Start()
    {
        random = Random.value;


    }

    // Update is called once per frame
    void Update()
    {
        if (bndCheck.offLeft || bndCheck.offRight)
            touchBound = true;
        if (bndCheck.offLeft)
        {
            touchLeftBound = true;
            touchRightBound = false;
        }
        else if (bndCheck.offRight)
        {
            touchLeftBound = false;
            touchRightBound = true;
        }
        Move();
    }
}
