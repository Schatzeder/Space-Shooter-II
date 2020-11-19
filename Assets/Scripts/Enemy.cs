using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _enemySpeed = 4.0f;
    
    //Deciding whether or not to shoot a laser on the first go
    [SerializeField]
    private int _laserID = 0;

    private bool _enemyDead = false;
    private bool _firing = false;

    [SerializeField]
    private GameObject _enemyLaser = null;

    private SpriteRenderer _enemyGun = null;
    [SerializeField]
    private Color _enemyGunColor;
    [SerializeField]
    private float _gunDelta = 0;
    [SerializeField]
    private Vector3[] _spawnPos = null;

    private UIManager _uiManager = null;
    private Player _player = null;

    private BoxCollider2D _enemyCollider = null;
    [SerializeField]
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

        Respawn();
    }

    void Respawn()
    {
        float randomX = Random.Range(-9f, 9f);
        float randomY = Random.Range(-6f, 9f);
        /*_spawnPos[0] = new Vector3(randomX, 7, 0);
        _spawnPos[1] = new Vector3(randomX, -6, 0);
        _spawnPos[2] = new Vector3(11.5f, randomY, 0);
        _spawnPos[3] = new Vector3(-11.5f, randomY, 0);*/

        transform.position = new Vector3(randomX, 7, 0);
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

    private IEnumerator EnemyFireLaser()
    {
        if (_enemyDead == false)
        {
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
}
