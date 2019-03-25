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
        if (_bndCheck.offUp || _bndCheck.offRight || _bndCheck.offLeft) //destroy if gone off screen from top
        {
            Destroy(gameObject);
        }
    }

    public void SetType(WeaponType eType) //set _type and color to match the weapon definition
    {
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        _rend.material.color = def.projectileColor;
    }
}
