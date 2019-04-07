using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum WeaponType
{
    simple,
    blaster,
    shield,
    Ally
}

[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.simple;
    public string letter; //letter to show on PowerUp
    public Color color = Color.white; //color of collar and PowerUp
    public GameObject projectilePrefab; //prefab for projectiles
    public GameObject allyPrefab; //prefab for ally ship
    public Color projectileColor = Color.white;
    public float damageOnHit = 0; //amount of damage
    public float continuousDamage = 0; //damage per sec
    public float delayBetweenShots = 0;
    public int activateAfterEnemyKill = 0; //weapon is activated after certain enemy kills
    public float velocity = 20; //speed of projectiles
}

public class Main : MonoBehaviour
{
    static public Main Singleton;
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float enemySpawnRateFactor = 90f; //used to determine the function: spawn interval = 2*e^2 / (time^2 + e^2)
    public float enemySpawnRateAfterBoss = 0.5f; //used to restrict enemy spawn rate
    public float enemyDefaultPadding = 1.5f;
    public float playTime = 60f; //playTime until final boss arrives
    public WeaponDefinition[] weaponDefinitions;
    public GameObject powerUpPrefab;
    public GameObject enemyBossPrefab;
    public WeaponType[] powerUpFrequency = new WeaponType[] { WeaponType.simple, WeaponType.simple, WeaponType.simple, //powerUp drop rate ratio is 3:3:2
                                                              WeaponType.blaster, WeaponType.blaster,WeaponType.blaster,
                                                              WeaponType.shield, WeaponType.shield };


    float _birthTime; //records start time of the game

    [HideInInspector]
    public bool _EnemyBossMode = true; //set enemy boss mode to false

    bool _EnemyBossSpawned = false;

    BoundsCheck _bndCheck;

    public void ShipDestroyed(Enemy e)
    {
        //potentially generate a powerUp
        if (Random.value <= e.powerUpDropChance)
        {
            //pick a powerup from powerUpFrequency array
            int ndx = Random.Range(0, powerUpFrequency.Length);
            WeaponType puType = powerUpFrequency[ndx];
            GameObject puGo = Instantiate(powerUpPrefab) as GameObject; //spawn powerup
            PowerUp pu = puGo.GetComponent<PowerUp>();
            pu.setType(puType); //set it to the proper weapon type
            pu.transform.position = e.transform.position; //set it to the position of the destroyed enemy
        }
    }


    // Start is called before the first frame update
    void Awake()
    {
        Singleton = this;
        _bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / 2); //invoke SpawnEnemy method every 2 sec

        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition weapDef in weaponDefinitions)
            WEAP_DICT[weapDef.type] = weapDef;

        _birthTime = Time.time;
    }

    public void SpawnEnemy()
    {
        int ndx = Random.Range(0, prefabEnemies.Length);
        GameObject go = Instantiate<GameObject>(prefabEnemies[ndx]);
        float enemyPadding = enemyDefaultPadding;
        if (go.GetComponent<BoundsCheck>() != null)
            enemyDefaultPadding = Mathf.Abs(go.GetComponent<BoundsCheck>().radius);

        Vector3 pos = Vector3.zero;
        float xMin = -_bndCheck.camWidth + enemyPadding;
        float xMax = _bndCheck.camWidth - enemyPadding;
        pos.x = Random.Range(xMin, xMax);
        pos.y = _bndCheck.camHeight + enemyPadding;
        go.transform.position = pos;
        if (!Hero.Ship.Win)
        {
            if (!_EnemyBossSpawned)
                Invoke("SpawnEnemy", Mathf.Pow(enemySpawnRateFactor, 2) * 2 / (Mathf.Pow(Time.time - _birthTime, 2) + enemySpawnRateFactor * enemySpawnRateFactor));
            else
                Invoke("SpawnEnemy", 1 / enemySpawnRateAfterBoss);
        }

    }

    void Update()
    {

        if (Time.time - _birthTime > playTime && _EnemyBossMode) //check if boss fight condition has met
        {
            Instantiate(enemyBossPrefab); //spawn boss and play boss background music
            AudioControl.AC.setAudioSourceClip("BGMBoss");
            AudioControl.AC.Play();
            _EnemyBossMode = false;
            _EnemyBossSpawned = true;
        }

        if (Hero.Ship.Win) //clear screen if player wins the game
        {
            ClearScreen();
        }
    }

    public void ClearScreen() //finds basically everything on screen and destroy them
    {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject go in gameObjects)
            Destroy(go);
        gameObjects = GameObject.FindGameObjectsWithTag("ProjectileEnemy");
        foreach (GameObject go in gameObjects)
            Destroy(go);
        gameObjects = GameObject.FindGameObjectsWithTag("PowerUp");
        foreach (GameObject go in gameObjects)
            Destroy(go);
        gameObjects = GameObject.FindGameObjectsWithTag("ProjectileAlly");
        foreach (GameObject go in gameObjects)
            Destroy(go);
        gameObjects = GameObject.FindGameObjectsWithTag("Ally");
        foreach (GameObject go in gameObjects)
            Destroy(go);
    }

    public void DelayedTransitionStartScreen(float delay) 
    {
        Invoke("StartScreen", delay); //invoke StartScrren method in delay sec
    }

    public void StartScreen()
    {
        SceneManager.LoadScene("_Scene_Start"); //load _Scene_Start to go to start screen of the game
    }

    public void DelayedRestart(float delay)
    {
        Invoke("Restart", delay); //invoke the restart method in delay sec
    }

    public void Restart()
    {
        SceneManager.LoadScene("_Scene_0"); //reload _Scene_0 to restart the game
    }

    static public WeaponDefinition GetWeaponDefinition(WeaponType wt)
    {
        if (WEAP_DICT.ContainsKey(wt))
            return WEAP_DICT[wt];
        return new WeaponDefinition();
    }
}
