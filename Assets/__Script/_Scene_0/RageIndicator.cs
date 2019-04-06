using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageIndicator : MonoBehaviour
{
    WeaponDefinition _wd;
    Vector3 _offset = new Vector3(0, 4.5f, 0);
    Renderer _rend;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = Hero.Ship.transform.position - _offset;
        transform.localScale = new Vector3(Hero.Ship.EnemyKill, transform.localScale.y, transform.localScale.z);
        _rend = GetComponent<Renderer>();
        _rend.material.color = Color.clear;
        _wd = Main.GetWeaponDefinition(WeaponType.Ally);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Hero.Ship.transform.position - _offset;
        transform.localScale = new Vector3(Hero.Ship.EnemyKill * 8 / _wd.activateAfterEnemyKill, transform.localScale.y, transform.localScale.z);
        if (Hero.Ship.EnemyKill >= _wd.activateAfterEnemyKill)
            transform.localScale = new Vector3(8, transform.localScale.y, transform.localScale.z);

        if (Hero.Ship.EnemyKill < _wd.activateAfterEnemyKill / 2)
            _rend.material.color = Color.red;
        else if (Hero.Ship.EnemyKill < _wd.activateAfterEnemyKill)
            _rend.material.color = Color.yellow;
        else
            _rend.material.color = Color.green;

            
        
    }
}
