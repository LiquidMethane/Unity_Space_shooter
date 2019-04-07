using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ally : MonoBehaviour
{
    [Header("Set In Insepctor")]
    public float fireRate; //ally ships' fire rate
    public float speed = 20f;
    public GameObject projectilePrefab;
    public float projectileSpeed = 60f;

    float _birthTime; 
    BoundsCheck _bndCheck;
    bool _reverse = false;

    // Start is called before the first frame update
    void Start()
    {
        _birthTime = Time.time;
        _bndCheck = GetComponent<BoundsCheck>();
        Invoke("Fire", 1 / fireRate); //begin shooting upon instantiation
    }

    // Update is called once per frame
    void Update()
    {

        if (transform.position.x > _bndCheck.camWidth - 2) //reverse direction if reaching the right bound
        {
            _reverse = true;
        }

        transform.position += Vector3.right * ((_reverse) ? -speed * Time.deltaTime : speed * Time.deltaTime);
        
        if (_bndCheck != null && _reverse && _bndCheck.offLeft) //destroys gameobject if out of left bounds
        {
            Destroy(gameObject);
        }

    }

    void Fire() //ally ships will fire continuously until destroyed
    {
        GameObject projGO = Instantiate<GameObject>(projectilePrefab);
        Projectile p = projGO.GetComponent<Projectile>();
        p.SetType(WeaponType.Ally);
        projGO.tag = "ProjectileAlly";
        projGO.transform.position = transform.position;
        projGO.GetComponent<Rigidbody>().velocity = Vector3.up * projectileSpeed;
        projGO.GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.yellow, Mathf.PingPong(Time.time, 1)); //changing projectiles' color between red and yellow
        Invoke("Fire", 1 / fireRate);
    }
}
