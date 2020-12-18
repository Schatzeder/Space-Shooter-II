using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4f;
    
    //Deciding whether or not to shoot a laser on the first go
    [SerializeField]
    private int _laserID = 0;
    [SerializeField]
    private int eType = 0;

    private bool _enemyDead = false;
    private bool _firing = false;

    [SerializeField]
    private GameObject _enemyLaser = null;

    private SpriteRenderer _enemyGun = null;
    [SerializeField]
    private Color _enemyGunColor;
    [SerializeField]
    private float _gunDelta = 0;
    //[SerializeField]
    //private Vector3[] _spawnPos = null;

    private UIManager _uiManager = null;
    private Player _player = null;
    private BoxCollider2D _enemyCollider = null;
    private Animator _enemyAnimator = null;
    private AudioSource _explosion = null;

    void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _player = GameObject.Find("Player").GetComponent<Player>();

        _enemyGunColor = this.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
        _enemyGun = this.transform.GetChild(0).GetComponent<SpriteRenderer>();

        _enemyCollider = GetComponent<BoxCollider2D>();
        _enemyAnimator = GetComponent<Animator>();
        _explosion = GetComponent<AudioSource>();

        _laserID = (Random.Range(0, 3));
        StartCoroutine(LaserRoutine());

        //Respawn();
    }

    void Respawn()
    {
        //RESET FIRING LOGIC
        float randomX = Random.Range(-9f, 9f);
        float randomY = Random.Range(-6f, 9f);
        /*_spawnPos[0] = new Vector3(randomX, 7, 0);
        _spawnPos[1] = new Vector3(randomX, -6, 0);
        _spawnPos[2] = new Vector3(11.5f, randomY, 0);
        _spawnPos[3] = new Vector3(-11.5f, randomY, 0);*/

        //BRING THIS BACK
        transform.position = new Vector3(randomX, 7, 0);

        //AIMBOT MODE:
            //Enemies spawn from all 4 sides
            //Enemies decide trajectory upon spawn
                //From spawn position, point toward player once, then travel forward until exiting screen
                //Respawn once @ end of screen (Do we need respawn method in AimBot Mode?)
            //Player measures enemies via an array system, I.E. Tower Defense method?
                //Array system takes in enemies that collide with Firing Radius
                //Player targets the enemy that entered the collider least recently (I.E. closest enemy first)
                //Player looks at targeted enemy and fires
                    //Real time player rotation? AKA not snapping??
                //Enemies remove themselves from array after dying, Player moves onto next closest enemy
            //Adjustable universal speed
    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = new Vector3(transform.position.x, transform.position.y, Mathf.Clamp(transform.position.z, 0, 0));

        transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);

        if (transform.position.y <= -4.5f && _enemyDead == false && _player._hellMode == false)
        {
            Respawn();
        }

        //LookAtDemo();
        //RotateDemo();
    }

    void RotateDemo()
    {
        /*if (eType == 1)
        {
            transform.Translate(Vector3.down * _enemySpeed * Time.deltaTime);
        }
        if (eType == 2)
        {
            transform.Translate(Vector3.up * _enemySpeed * Time.deltaTime);
        }*/
        if (eType == 3)
        {
            transform.Translate(Vector3.left * _enemySpeed * Time.deltaTime);
        }
        else if (eType == 4)
        {
            transform.Translate(Vector3.right * _enemySpeed * Time.deltaTime);
        }
    }

    void LookAtDemo()
    {
        Vector3 target = _player.transform.position;
        Vector3 player = this.transform.position;

        float xDiff = player.x - target.x;
        float yDiff = player.y - target.y;
        Debug.Log("xDiff = " + xDiff);

        float ATan = Mathf.Atan(yDiff / xDiff) * Mathf.Rad2Deg;
        float ATan2 = Mathf.Atan2(yDiff, xDiff) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, ATan2 - 90));
        transform.rotation = targetRotation;
    }




    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            
            if (player != null)
            {
                player.Damage();
            }

            EnemyDeath();
        }
        else if (other.tag == "Laser")
        {
            _uiManager.UpdateScore(10);
            Destroy(other.gameObject);

            EnemyDeath();
        }
        else if (other.tag == "Asteroid")
        {
            //Logic for rogue asteroid collisions
            EnemyDeath();
        }
    }

    private void EnemyDeath()
    {
        _enemyDead = true;
        _enemyGun.enabled = !_enemyGun.enabled;
        //this.transform.GetChild(0).GetComponent<SpriteRenderer>().color = _enemyGunColor;
        _explosion.Play();
        _enemySpeed *= 0.6f;
        _enemyCollider.enabled = !_enemyCollider.enabled;
        _enemyAnimator.SetBool("OnEnemyDeath", true);
        Destroy(this.gameObject, 2.45f);
    }

    /*
    private IEnumerator LaserRoutine()
    {
        if (_laserID == 2)
        {
            _firing = true;
            yield return new WaitForSeconds(Random.Range(0.5f, 1.5f));
            StartCoroutine(EnemyFireLaser());
            yield return new WaitForSeconds(1.5f);
            _firing = false;
        }

        while (true)
        {
            if (_firing == false)
            {
                yield return new WaitForSeconds(Random.Range(1.5f, 5.0f));
                StartCoroutine(EnemyFireLaser());
            }
        }
    }
    */

    protected virtual IEnumerator LaserRoutine()
    {   //Repeat always while enemy is alive. CHANGE FOR RESPAWN???
        while (_enemyDead == false)
        {   //Fire only if you aren't preparing to do so
            if (_firing == false)
            {
                _firing = true;
                yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
                //Get Handle on Thruster Sprite
                for (int i = 0; i < 12; i++)
                {
                    _gunDelta += 0.025f;
                    //NONOPTIMAL -- Using GetComponent repeatedly
                    this.transform.GetChild(0).GetComponent<SpriteRenderer>().color += new Color(0, 0, 0, _gunDelta);
                    yield return new WaitForSeconds(0.1f);

                    if (_gunDelta >= 0.3f)
                    {
                        GameObject eLaser = Instantiate(_enemyLaser, transform.position + new Vector3(0, -0.5f, 0), Quaternion.identity, this.transform);
                        eLaser.GetComponent<Laser>()._laserID = 2;
                        _gunDelta = 0;
                        this.transform.GetChild(0).GetComponent<SpriteRenderer>().color = _enemyGunColor;
                    }
                }
            }
        }
    }
}
