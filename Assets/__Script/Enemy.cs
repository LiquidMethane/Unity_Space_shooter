using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector:Enemy")]
    public float speed = 10f;

    protected BoundsCheck bndCheck;
    

    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }
    public Vector3 Pos
    {
        get { return (transform.position);  }
        set { transform.position = value; }
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    
    public virtual void Move()
    {
        Vector3 tempPos = Pos;
        tempPos.y -= speed * Time.deltaTime; //moves enemy downward 
        Pos = tempPos;

        if (bndCheck != null && (bndCheck.offDown || bndCheck.offLeft || bndCheck.offRight)) //destroys enemy object if out of bounds
        {
            Destroy(gameObject);
        }
    }
}
