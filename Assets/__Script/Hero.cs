using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero Ship;

    [Header("Set in Inspector")]
    public float speed = 30; //speed factor larger means herp moves faster
    public float rollMult = -45;
    public float pitchMult = 30;
    public float gameRestartDelay = 2f; //second which game restarts in when player dies

    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 1;

    private GameObject _lastTriggerGo = null;


    private void Awake() // set the ship singleton
    {
        if (Ship == null)
            Ship = this;
        else
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S");
    }

    // Update is called once per frame
    void Update()
    {
        float xAxis = Input.GetAxis("Horizontal"); //get input from player
        float yAxis = Input.GetAxis("Vertical");

        Vector3 pos = transform.position;

        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        //rotate the ship to make moves more dynamic
        transform.rotation = Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform _rootT = other.gameObject.transform.root;
        GameObject _go = _rootT.gameObject;

        if (_go == _lastTriggerGo) //make sure its not the same Enemy as last time
            return;
        _lastTriggerGo = _go;

        if (_go.tag == "Enemy") //decrease shieldLevel and destroy enemy when hit
        {
            ShieldLevel--;
            Destroy(_go);
        }
        else
        {
            print("Triggered by non-Enemy" + _go.name);
        }

    } 

    public float ShieldLevel
    {
        get
        {
            return _shieldLevel;
        }
        set
        {
            _shieldLevel = Mathf.Min(value, 4);

            if (value < 0) //if shield is gone destroy Hero
            {
                Destroy(this.gameObject);
                Main.Singleton.DelayedRestart(gameRestartDelay);
            }
        }
    }
}
