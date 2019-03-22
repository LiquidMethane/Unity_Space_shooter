using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum WeaponType
{
    simple,
    blaster
}

[System.Serializable]
public class WeaponDefinition
{
    public WeaponType type = WeaponType.simple;
    public string letter; //letter to show on PowerUp
    public Color color = Color.white; //color of collar and PowerUp
    public GameObject projectilePrefab; //prefab for projectiles
    public Color projectileColor = Color.white;
    public float damageOnHit = 0; //amount of damage
    public float continuousDamage = 0; //damage per sec
    public float delayBetweenShots = 0;
    public float velocity = 20; //speed of projectiles
}

public class Main : MonoBehaviour
{
    static public Main Singleton;
    static Dictionary<WeaponType, WeaponDefinition> WEAP_DICT;

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyDefaultPadding = 1.5f;
    public WeaponDefinition[] weaponDefinitions;

    BoundsCheck _bndCheck;

    // Start is called before the first frame update
    void Awake()
    {
        Singleton = this;
        _bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond); //invoke SpawnEnemy method every 2 sec

        WEAP_DICT = new Dictionary<WeaponType, WeaponDefinition>();
        foreach (WeaponDefinition weapDef in weaponDefinitions)
            WEAP_DICT[weapDef.type] = weapDef; 
    }

    public void SpawnEnemy()
    {
        int ndx = Random.Range(0,prefabEnemies.Length);
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
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond); //invoke SpawnEnemy method again every 2 sec
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
