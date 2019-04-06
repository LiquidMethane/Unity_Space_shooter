using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [Header("Set In Inspector")]
    public float speed = 10;

    static public Boss B;
    public Part[] parts;
    private BoundsCheck _bndCheck;
    private Rigidbody _rigid;
    private Vector3 _currentDir;
    private bool enterScreen = false;

    // Start is called before the first frame update
    private void Awake()
    {
        if (B == null)
        {
            B = this;
        }
        else
        {
            Debug.LogError("Boss.Awake()-Attemp to assign second Boss.B!");
        }

        _bndCheck = GetComponent<BoundsCheck>();
        _rigid = GetComponent<Rigidbody>();
    }
    public Vector3 pos
    {
        get { return (this.transform.position); }
        set { this.transform.position = value; }
    }


    void Start()
    {
        Transform t;
        foreach (Part prt in parts)
        {
            t = transform.Find(prt.name);
            if (t != null)
            {
                prt.go = t.gameObject;
            }
        }
        enterScreen = false;
        _currentDir = new Vector3(Random.Range(-1f, 1f), -1f, 0);
        _currentDir.Normalize();
        transform.position += _currentDir * speed * Time.deltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < 30)
            enterScreen = true;
        Move();
        if (parts[0].health >= 0) { parts[0].go.transform.Rotate(new Vector3(0, 0, 30) * Time.deltaTime); }
        if (parts[1].health >= 0) { parts[1].go.transform.Rotate(new Vector3(0, 0, -30) * Time.deltaTime); }

    }

    public void Move()
    {
        if (enterScreen)
        {
            if (_bndCheck.offLeft)
            {
                _currentDir = RandomDirection(1, 0);
            }

            else if (_bndCheck.offRight)
            {
                _currentDir = RandomDirection(-1, 0);
            }

            else if (_bndCheck.offUp)
            {
                _currentDir = RandomDirection(0, -1);
            }

            else if (_bndCheck.offDown)
            {
                _currentDir = RandomDirection(0, 1);
            }
        }

        transform.position += _currentDir * speed * Time.deltaTime;
    }

    Vector3 RandomDirection(int horz, int vert)
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
        
        prt.health -= damage;
        Destroy(otherGo);
        if (prt.health < 0)
        {
            Destroy(otherGo);
            Destroy(prt.go);
            Main.Singleton.audioSource.PlayOneShot(Main.Singleton.blast, Random.Range(0.5f, 1f));
            if (prt.name == "core")
            {
                Destroy(prt.go.transform.parent.gameObject);
                Hero.Ship.Victory();
            }
                
        }
        else
        {
            Main.Singleton.audioSource.PlayOneShot(Main.Singleton.hit, Random.Range(0.3f, 0.6f));
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
                float damage = Main.GetWeaponDefinition(p.type).damageOnHit * ((p.tag == "ProjectileAlly") ? 0.5f : 1f);

                DamagePart(parts[0], otherGO, damage);
                if (parts[0].health < 0)
                {
                    DamagePart(parts[1], otherGO, damage);
                }
                if (parts[0].health < 0 && parts[1].health < 0)
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

