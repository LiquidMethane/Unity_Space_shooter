using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private BoundsCheck _bndCheck;
    private Renderer _rend;

    [Header("Set Dynamically")]
    public Rigidbody rigid;
    [SerializeField]
    private WeaponType _type;


    public WeaponType type
    {
        get
        { return _type; }
        set
        { SetType(value); }
    }

    private void Awake()
    {
        _bndCheck = GetComponent<BoundsCheck>();
        _rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameObject.tag)
        {
            case "ProjectileHero":
            case "ProjectileAlly":
                if (_bndCheck.offUp || _bndCheck.offRight || _bndCheck.offLeft) //destroy if gone off screen from top
                {
                    Destroy(gameObject);
                }
                break;

            case "ProjectileEnemy":
                if (_bndCheck.offDown || _bndCheck.offRight || _bndCheck.offLeft) 
                {
                    Destroy(gameObject);
                }
                break;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;
        if ((gameObject.tag == "ProjectileHero" || gameObject.tag == "ProjectileAlly") && go.tag == "ProjectileEnemy")
        {
            Destroy(gameObject);
            Destroy(go);
        }
        else if ((go.tag == "ProjectileHero" || go.tag == "ProjectileAlly") && gameObject.tag == "ProjectileEnemy")
        {
            Destroy(gameObject);
            Destroy(go);
        }
    }


    public void SetType(WeaponType eType) //set _type and color to match the weapon definition
    {
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        _rend.material.color = def.projectileColor;
    }
}
