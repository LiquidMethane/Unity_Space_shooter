using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector:Enemy")]
    public float speed = 10f;
    public float health = 1f;
    public int score = 1;

    protected BoundsCheck bndCheck;
    

    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
    }

    protected virtual void Start() //determine health and score for every enemy upon spawning
    {
        float _rand = Random.value;
        if (_rand < 0.5f)
        {
            health = 1f;
            score = 1;
        }
            
        else
        { 
            health = 3f;
            score = 3;
        }
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

    private void OnCollisionEnter(Collision collision)
    {
        GameObject otherGO = collision.gameObject;
        switch(otherGO.tag)
        {
            case "ProjectileHero":
                Projectile p = otherGO.GetComponent<Projectile>();
                if (!bndCheck.isOnScreen) //if the enemy is off screen, no damage is done
                {
                    Destroy(otherGO);
                    break;
                }

                health -= Main.GetWeaponDefinition(p.type).damageOnHit;//reduce enemy's health by the amount acquired from WEAP_DICT

                if (health == 0)
                {
                    Destroy(gameObject);
                    ScoreManager.Singleton.Score += score;
                    ScoreManager.Singleton.DisplayScore();
                }
                Destroy(otherGO);
                break;

            default:
                print("Enemy hit by non-ProjectileHero: " + otherGO.name);
                break; 
        }
    }
}
