using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour
{
    [Header("Set in Inspector")]
    public float rotationPerSecond = 0.1f;

    [Header("Set Dynamically")]
    public int levelShown = 0;

    Material _mat;
    
    // Start is called before the first frame update
    void Start()
    {
        _mat = GetComponent<Renderer>().material;
    }

    // Update is called once per frame
    void Update()
    {
        int currLevel = Mathf.FloorToInt(Hero.Ship.ShieldLevel); // read current shield level from hero.ship singleton

        if (levelShown != currLevel) 
        {
            levelShown = currLevel;
            _mat.mainTextureOffset = new Vector2(levelShown * 0.2f, 0); //adjust the texture to represent the current shield level
        }

        float _rotateZ = -(rotationPerSecond * Time.time * 360) % 360f; //rotate the shield a little every frame in a time-based way

        transform.rotation = Quaternion.Euler(0, 0, _rotateZ);
    }
}
