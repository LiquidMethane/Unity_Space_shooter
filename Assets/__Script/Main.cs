using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    static public Main Singleton;

    [Header("Set in Inspector")]
    public GameObject[] prefabEnemies;
    public float enemySpawnPerSecond = 0.5f;
    public float enemyDefaultPadding = 1.5f;

    BoundsCheck _bndCheck;

    // Start is called before the first frame update
    void Start()
    {
        Singleton = this;
        _bndCheck = GetComponent<BoundsCheck>();
        Invoke("SpawnEnemy", 1f / enemySpawnPerSecond); //invoke SpawnEnemy method every 2 sec
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

}
