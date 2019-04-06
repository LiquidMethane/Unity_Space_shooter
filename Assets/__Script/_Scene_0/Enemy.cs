using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Set in Inspector:Enemy")]
    public float speed = 10f;
    public float health = 1f;
    public int score = 1;
    public float powerUpDropChance = 1f; //chance to drop a powerup
    public float fireRate;
    public float bulletVelocity;
    public GameObject projectilePrefab;
    
    

    protected BoundsCheck bndCheck;
    private bool acceptDamage = true;
    private AudioSource _audioSource;

    private void Awake()
    {
        bndCheck = GetComponent<BoundsCheck>();
        _audioSource = Main.Singleton.audioSource;
    }

    void Start() //determine health and score for every enemy upon spawning
    {
        health = 1f;
        score = 1;
        Invoke("Fire", 0.5f);
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
            case "ProjectileAlly":
            case "ProjectileHero":
                Projectile p = otherGO.GetComponent<Projectile>();
                if (!bndCheck.isOnScreen) //if the enemy is off screen, no damage is done
                {
                    Destroy(otherGO);
                    break;
                }

                health -= Main.GetWeaponDefinition(p.type).damageOnHit;//reduce enemy's health by the amount acquired from WEAP_DICT
                if (tag == "ProjectileAlly")
                    print(Main.GetWeaponDefinition(p.type).damageOnHit);

                if (acceptDamage & health <= 0)
                {
                    acceptDamage = false;
                    Main.Singleton.ShipDestroyed(this);
                    Destroy(gameObject);
                    if (otherGO.tag == "ProjectileHero")
                    {
                        Hero.Ship.EnemyKill++;
                        if (Hero.Ship.EnemyKill == Main.GetWeaponDefinition(WeaponType.Ally).activateAfterEnemyKill)
                            _audioSource.PlayOneShot(Main.Singleton.reload);
                    }
                        
                    ScoreManager.Singleton.Score += score;
                    ScoreManager.Singleton.DisplayScore();

                    _audioSource.PlayOneShot(Main.Singleton.blast, Random.Range(0.5f, 1f));
                }
                else
                {
                    if (otherGO.tag == "ProjectileHero")
                        _audioSource.PlayOneShot(Main.Singleton.hit, Random.Range(0.4f, 0.6f));
                }
                Destroy(otherGO);
                break;

            default:
                print("Enemy hit by non-ProjectileHero: " + otherGO.name);
                break; 
        }
    }

    protected void Fire()
    {
        GameObject projGO = Instantiate<GameObject>(projectilePrefab);
        projGO.GetComponent<Renderer>().material.color = Color.yellow;
        projGO.tag = "ProjectileEnemy";
        projGO.layer = LayerMask.NameToLayer("ProjectileEnemy"); ;
        projGO.transform.position = transform.position;
        projGO.GetComponent<Rigidbody>().velocity = Vector3.down * bulletVelocity;
        Invoke("Fire", 1 / fireRate);
    }
}
