using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    static public Transform PROJECTILE_ANCHOR;

    [Header("Set Dynamically")]
    [SerializeField]
    private WeaponType _type = WeaponType.simple; //set Weapon to simple by default
    public WeaponDefinition def;
    public GameObject collar;
    public float lastShotTime;

    private Renderer _collarRend;
    private WeaponType _lastWeapon;
    private AudioSource _audioSource;

    // Start is called before the first frame update
    void Start()
    {
        collar = transform.Find("Collar").gameObject;
        _collarRend = collar.GetComponent<Renderer>();

        SetType(_type);

        if (PROJECTILE_ANCHOR == null) // create an anchor for all projectiles
        {
            GameObject go = new GameObject("_ProjectileAnchor");
            PROJECTILE_ANCHOR = go.transform;
        }

        GameObject rootGO = transform.root.gameObject; // find the fireDelegate of root gameobject
        if (rootGO.GetComponent<Hero>() != null)
        {
            rootGO.GetComponent<Hero>().fireDelegate += Fire;
        }
        _audioSource = Main.Singleton.audioSource;
    }

    public WeaponType type
    {
        get { return _type; }
        set { SetType(value); }
    }

    public void SetType(WeaponType wt)
    {
        _type = wt;
        this.gameObject.SetActive(true);
        def = Main.GetWeaponDefinition(_type);
        _collarRend.material.color = def.color;
        lastShotTime = 0; // can fire immediately after _type is set
    }

    public void Fire()
    {
        if (!gameObject.activeInHierarchy) return; //return if gameobject is inactive
        if (Time.time - lastShotTime < def.delayBetweenShots) return; //return if fire delay is not met
        if (type == WeaponType.Ally) //return if enemykill amount is not met
        {
            if (def.activateAfterEnemyKill > Hero.Ship.EnemyKill)
                return;
        }
        Projectile p;
        Vector3 vel = Vector3.up * def.velocity;
        if (transform.up.y < 0)
        {
            vel.y = -vel.y;
        }

        switch (type)
        {
            case WeaponType.simple:
                _audioSource.PlayOneShot(Main.Singleton.standardWeapon, Random.Range(0.5f, 1f));
                p = MakeProjectile();
                p.rigid.velocity = vel;
                break;
            case WeaponType.blaster:
                _audioSource.PlayOneShot(Main.Singleton.blasterWeapon, Random.Range(0.5f, 1f));
                p = MakeProjectile(); //middle projectile
                p.rigid.velocity = vel;
                p = MakeProjectile(); //right projectile
                p.transform.rotation = Quaternion.AngleAxis(30, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                p = MakeProjectile(); //left projectile
                p.transform.rotation = Quaternion.AngleAxis(-30, Vector3.back);
                p.rigid.velocity = p.transform.rotation * vel;
                break;
            case WeaponType.Ally:
                _audioSource.PlayOneShot(Main.Singleton.machineGun, 0.3f);
                SummonAlly();
                break;
        }
    }

    public void SummonAlly()
    {
        Hero.Ship.EnemyKill = 0;
        GameObject allyGO;
        Vector3 offset = new Vector3(6, 0, 0);
        for (int i = 0; i < 5; i++)
        {
            allyGO = Instantiate(def.allyPrefab) as GameObject;
            allyGO.transform.position += new Vector3(-35, -35, 0) - i * offset;
        }
        lastShotTime = Time.time;
    }

    public Projectile MakeProjectile()
    {
        GameObject go = Instantiate<GameObject>(def.projectilePrefab);
        if (transform.parent.gameObject.tag == "Hero")
        {
            go.tag = "ProjectileHero";
            go.layer = LayerMask.NameToLayer("ProjectileHero");
        }

        go.transform.position = collar.transform.position;
        go.transform.SetParent(PROJECTILE_ANCHOR, true);
        Projectile p = go.GetComponent<Projectile>();
        p.type = type;
        lastShotTime = Time.time;
        return p;
    }

    void Update()
    {
        SwitchWeapon();
    }

    void SwitchWeapon() //switch weapon to corresponding keypress
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && _lastWeapon != WeaponType.simple)
        {
            SetType(WeaponType.simple);
            _lastWeapon = WeaponType.simple;
        }
            
        if (Input.GetKeyDown(KeyCode.Alpha2) && _lastWeapon != WeaponType.blaster)
        {
            SetType(WeaponType.blaster);
            _lastWeapon = WeaponType.blaster;
        }
            
        if (Input.GetKeyDown(KeyCode.Alpha3) && _lastWeapon != WeaponType.Ally)
        {
            SetType(WeaponType.Ally);
            _lastWeapon = WeaponType.Ally;
        }
            
    }
}
