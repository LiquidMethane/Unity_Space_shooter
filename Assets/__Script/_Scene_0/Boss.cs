using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Set In Inspector")]
    public float speed = 10;

    static public Boss B; //boss singleton
    public Part[] parts; //parts array stores all the parts of boss enemy
    private BoundsCheck _bndCheck;
    private Rigidbody _rigid;
    private Vector3 _currentDir;
    private bool enterScreen = false;
    private int _score;

    // Start is called before the first frame update
    private void Awake()
    {
        if (B == null) //assign singleton
        {
            B = this;
        }
        else
        {
            Debug.LogError("Boss.Awake()-Attemp to assign second Boss.B!");
        }

        _bndCheck = GetComponent<BoundsCheck>();
        _bndCheck.radius = 10;
        _rigid = GetComponent<Rigidbody>();
    }

    void Start()
    {
        Transform t;
        foreach (Part prt in parts) //assigning part to parts array
        {
            t = transform.Find(prt.name);
            if (t != null)
            {
                prt.go = t.gameObject;
                _score += (int)prt.health;
            }
        }
        enterScreen = false;
        _currentDir = new Vector3(Random.Range(-1f, 1f), -1f, 0); //select a random downward direction
        _currentDir.Normalize();
        transform.position += _currentDir * speed * Time.deltaTime; //move boss in this direction
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < _bndCheck.camHeight - 10) //check is boss fully enters the screen
            enterScreen = true;

        Move();

        if (parts[0].health > 0) { parts[0].go.transform.Rotate(new Vector3(0, 0, 30) * Time.deltaTime); } //rotate outer ring
        if (parts[1].health > 0) { parts[1].go.transform.Rotate(new Vector3(0, 0, -30) * Time.deltaTime); } //rotate inner ring

    }

    public void Move()
    {
        if (enterScreen) //if boss is fully in the screen
        {
            if (_bndCheck.offLeft) //if boss touches left bound, reset direction to a random rightward direction
            {
                _currentDir = GetRandomDirection(1, 0);
            }

            else if (_bndCheck.offRight) //if boss touches right bound, reset direction to a random leftward directon
            {
                _currentDir = GetRandomDirection(-1, 0);
            }

            else if (_bndCheck.offUp) //if boss touches upper bound, reset direction to a random downward direction
            {
                _currentDir = GetRandomDirection(0, -1);
            }

            else if (_bndCheck.offDown) //if boss touches lower bound, reset direction to a random upward direction
            {
                _currentDir = GetRandomDirection(0, 1);
            }
        }

        transform.position += _currentDir * speed * Time.deltaTime; //move in this directoin
    }

    Vector3 GetRandomDirection(int horz, int vert) //helper function: -1 means to generate negative random value, 0 means sign doesn't matter, 1 means positive random number
    {
        if (horz == 1)
        {
            if (vert == 1)
            {
                return Vector3.Normalize(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), 0));
            }
            else if (vert == 0)
            {
                return Vector3.Normalize(new Vector3(Random.Range(0f, 1f), Random.Range(-1f, 1f), 0));
            }
            else
            {
                return Vector3.Normalize(new Vector3(Random.Range(0f, 1f), Random.Range(0f, -1f), 0));
            }
        }
        else if (horz == 0)
        {
            if (vert == 1)
            {
                return Vector3.Normalize(new Vector3(Random.Range(-1f, 1f), Random.Range(0f, 1f), 0));
            }
            else if (vert == 0)
            {
                return Vector3.Normalize(new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0));
            }
            else
            {
                return Vector3.Normalize(new Vector3(Random.Range(-1f, 1f), Random.Range(0f, -1f), 0));
            }
        }
        else
        {
            if (vert == 1)
            {
                return Vector3.Normalize(new Vector3(Random.Range(0f, -1f), Random.Range(0f, 1f), 0));
            }
            else if (vert == 0)
            {
                return Vector3.Normalize(new Vector3(Random.Range(0f, -1f), Random.Range(-1f, 1f), 0));
            }
            else
            {
                return Vector3.Normalize(new Vector3(Random.Range(0f, -1f), Random.Range(0f, -1f), 0));
            }
        }

    }

    private void DamagePart(Part prt, GameObject otherGo, float damage)
    {
        
        prt.health -= damage; //damage the part
        Destroy(otherGo);
        if (prt.health <= 0) //destroy the part if health < 0
        {
            Destroy(prt.go);
            AudioControl.AC.PlayOneShot("blast", 05f, 1f);
            if (prt.name == "OuterRing")
                _bndCheck.radius = 7;
            else if (prt.name == "InnerRing")
                _bndCheck.radius = 3;
            else if (prt.name == "Core")
            {
                ScoreManager.Singleton.Score += _score;
                ScoreManager.Singleton.DisplayScore();
                Destroy(prt.go.transform.parent.gameObject); //if Core health < 0 destroy boss gameobject and claim victory
                Hero.Ship.Victory();
            }
                
        }
        else
        {
            AudioControl.AC.PlayOneShot("hit", 0.3f, 0.6f);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        GameObject otherGO = collision.gameObject;

        switch (otherGO.tag)
        {
            case "ProjectileAlly": 
            case "ProjectileHero":
                Projectile p = otherGO.GetComponent<Projectile>();
                float damage = Main.GetWeaponDefinition(p.type).damageOnHit * ((p.tag == "ProjectileAlly") ? 0.5f : 1f); //if collides with ally projectiles damage is reduced to half of the amount

                if (parts[0].health > 0) //damage dealt from outer ring to inner ring to core
                {
                    DamagePart(parts[0], otherGO, damage);
                }
                else if (parts[0].health <= 0 && parts[1].health > 0)
                {
                    DamagePart(parts[1], otherGO, damage);
                }
                else
                {
                    DamagePart(parts[2], otherGO, damage);
                }
                break;

            default:
                print("Enemy hit by non-Projectile" + otherGO.name);
                break;
        }
    }
}

