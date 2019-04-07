using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageIndicator : MonoBehaviour
{
    WeaponDefinition _wd;
    Vector3 _offset = new Vector3(0, 4.5f, 0); //offset makes indicator bar sit at the bottom of the hero ship
    Renderer _rend;

    // Start is called before the first frame update
    void Start()
    {
        _wd = Main.GetWeaponDefinition(WeaponType.Ally);
        transform.position = Hero.Ship.transform.position - _offset; //set indicator to the bottom of the hero ship
        transform.localScale = new Vector3(Hero.Ship.enemyKill * 8 / _wd.activateAfterEnemyKill, transform.localScale.y, transform.localScale.z); //set the indicator to appropriate length
        _rend = GetComponent<Renderer>();
        _rend.material.color = Color.clear; //set indicator to clear
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Hero.Ship.transform.position - _offset; //keeps indicator to the bottom of the hero ship
        transform.localScale = new Vector3(Hero.Ship.enemyKill * 8 / _wd.activateAfterEnemyKill, transform.localScale.y, transform.localScale.z); //set the indicator to appropriate length
        if (Hero.Ship.enemyKill >= _wd.activateAfterEnemyKill)
            transform.localScale = new Vector3(8, transform.localScale.y, transform.localScale.z); //if enemyKill reaches activation requirement, set indicator to fixed length 8

        if (Hero.Ship.enemyKill < _wd.activateAfterEnemyKill / 2) //if less than half of progress is made, color is red
            _rend.material.color = Color.red;
        else if (Hero.Ship.enemyKill < _wd.activateAfterEnemyKill) //if more than half of the progress but not all is made, color is yellow
            _rend.material.color = Color.yellow;
        else
            _rend.material.color = Color.green; //if ready to fire, color is green

            
        
    }
}
