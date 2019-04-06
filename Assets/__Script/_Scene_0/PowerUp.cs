using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [Header("Set In Inspector")]
    public Vector2 rotMinMax = new Vector2(15, 90); //holds min and max value for random number generation
    public Vector2 driftMinMax = new Vector2(.25f, 2);
    public float lifeTime = 5f; //seconds the powerup exists
    public float fadeTime = 3f; //sconds from fade to disappear

    [Header("Set Dynamically")]
    public WeaponType type; //type of the powerup
    public GameObject cube; //reference to the cube child
    public TextMesh letter; //reference to the textMesh
    public Vector3 rotPerSec; //Euler rotation speed
    public float birthTime;

    private Rigidbody _rigid;
    private BoundsCheck _bndCheck;
    private Renderer _cubeRend;


    void Awake()
    {
        cube = transform.Find("Cube").gameObject; //find the cube reference
        letter = GetComponent<TextMesh>(); //find other components
        _rigid = GetComponent<Rigidbody>();
        _bndCheck = GetComponent<BoundsCheck>();
        _cubeRend = cube.GetComponent<Renderer>();

        Vector3 vel = Random.onUnitSphere; //set a random velocity
        vel.z = 0;
        vel.Normalize(); //normalizes the length to 1
        vel *= Random.Range(driftMinMax.x, driftMinMax.y);
        _rigid.velocity = vel;

        transform.rotation = Quaternion.identity; //set the rotation of powerup to R:[0,0,0]

        //set up the rotPerSec for the cube child using rotMinMax
        rotPerSec = new Vector3(Random.Range(rotMinMax.x, rotMinMax.y), Random.Range(rotMinMax.x, rotMinMax.y), Random.Range(rotMinMax.x, rotMinMax.y));

        birthTime = Time.time; //set birth time
    }

    void Update()
    {
        cube.transform.rotation = Quaternion.Euler(rotPerSec * Time.time);

        float life = (Time.time - birthTime - lifeTime) / fadeTime; //determine if powerup needs to go
        //in lifetime, life will be <= 0, then transition to 1 over the course of fadetime, obejct will be destroyed when life >= 1

        if (life >= 1)
        {
            Destroy(this.gameObject);
            return;
        }

        if (life > 0) //use life to determine the alpha value of cube and letter
        {
            Color c = _cubeRend.material.color;
            c.a = 1f - life;
            _cubeRend.material.color = c;

            c = letter.color; //also fade out letter, not as much as cube
            c.a = 1f - (life * 0.5f);
            letter.color = c;
        }

        if (!_bndCheck.isOnScreen) //destroy powerup if it drifts off the screen
        {
            Destroy(gameObject);
        }
    }

    public void setType(WeaponType wt)
    {
        WeaponDefinition def = Main.GetWeaponDefinition(wt); //grab weaponDefiniton from Main
        _cubeRend.material.color = def.color; //set the color of the cube
        letter.text = def.letter; //set letter
        type = wt; //set type
    }

    public void AbsorbedBy(GameObject target) //this function is called by Hero class when PowerUp is collected
    {
        Main.Singleton.audioSource.PlayOneShot(Main.Singleton.levelUp);
        Destroy(this.gameObject);
    }
}
