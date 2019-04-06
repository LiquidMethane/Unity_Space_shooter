using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour
{
    static public Hero Ship;

    [Header("Set in Inspector")]
    public float speed = 30; //speed factor: larger means herp moves faster
    public float rollMult = -45;
    public float pitchMult = 30;
    public float gameRestartDelay = 5f; //second which game restarts in when player dies
    public GameObject rageIndicatorPrefab;
    
    [Header("Set Dynamically")]
    [SerializeField]
    private float _shieldLevel = 1;

    private GameObject _lastTriggerGo = null;
    

    public delegate void WeaponFireDelegate();  //delegate to fire weapon

    public WeaponFireDelegate fireDelegate;
    private GameObject _rageIndicator;
    private AudioSource _audioSource;
    private bool win = false;

    public bool Win
    {
        get { return win; }
        set { win = value; }
    }

    float _enemyKill = 0;

    public float EnemyKill
    {
        get{ return _enemyKill; }
        set { _enemyKill = value; }
    }

    private void Awake() // set the ship singleton
    {
        if (Ship == null)
            Ship = this;
        else
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S");

        _rageIndicator = Instantiate(rageIndicatorPrefab);
        _audioSource = Main.Singleton.audioSource;
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

        if (Input.GetAxis("Jump") == 1 && fireDelegate != null) //hold space bar to fire weapon
        {
            fireDelegate();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform _rootT = other.gameObject.transform.root;
        GameObject _go = _rootT.gameObject;

        if (_go == _lastTriggerGo) //make sure its not the same Enemy as last time
            return;
        _lastTriggerGo = _go;

        if (_go.tag == "Enemy" || _go.tag == "ProjectileEnemy") //decrease shieldLevel and destroy enemy when hit
        {
            ShieldLevel--;
            if (_shieldLevel >= 0)
            {
                _audioSource.PlayOneShot(Main.Singleton.shieldBlop, 1f);
            }
                
            Destroy(_go);
        }
        else if (_go.tag == "EnemyBoss")
        {
            GameOver();
        }
        else if (_go.tag == "PowerUp") //if triggered by powerup
        {
            AbsorbPowerUp(_go);
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
                GameOver();
            }
        }
    }

    public void AbsorbPowerUp(GameObject go)
    {
        
        PowerUp pu = go.GetComponent<PowerUp>(); //get powerup component

        switch (pu.type)
        {
            
            case WeaponType.shield: //restore full shield 
                ShieldLevel = 4;
                break;
                
            default:
                WeaponDefinition wd = Main.GetWeaponDefinition(pu.type);
                if (pu.type == WeaponType.blaster)//increase blaster firing rate then increase damage
                {
                    if (wd.delayBetweenShots >= 0.3f)
                        wd.delayBetweenShots -= wd.delayBetweenShots * 0.15f;
                    else
                        wd.damageOnHit += wd.damageOnHit * 0.1f;
                }
                    

                else if (pu.type == WeaponType.simple) //increase damage for simple weapon type then increase firing rate
                {
                    if (wd.damageOnHit <= 3)
                        wd.damageOnHit += wd.damageOnHit * 0.25f;
                    else
                        wd.delayBetweenShots -= wd.delayBetweenShots * 0.05f;
                }
                break;

        }
        pu.AbsorbedBy(gameObject);
    }

    public void GameOver()
    {
        _audioSource.Stop();
        _audioSource.PlayOneShot(Main.Singleton.failure, 1f);
        Destroy(gameObject);
        Destroy(_rageIndicator);
        ScoreManager.Singleton.SaveHighScore();
        Main.Singleton.DelayedRestart(gameRestartDelay);
    }

    public void Victory()
    {
        win = true;
        _audioSource.Stop();
        _audioSource.PlayOneShot(Main.Singleton.victory, 1f);
        ScoreManager.Singleton.SaveHighScore();
        Main.Singleton.DelayedTransitionStartScreen(gameRestartDelay);
    }
}
